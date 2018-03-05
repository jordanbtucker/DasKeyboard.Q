using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace DasKeyboard.Q
{
    /// <summary>
    /// Represents a client that accesses the Q REST API.
    /// </summary>
    public class Client
    {
        private HttpClient httpClient = new HttpClient();

        private DateTime lastRefreshTime;

        /// <summary>
        /// The authorization information to use when authenticating with the
        /// Q REST API. This information is automatically updated by the
        /// client, but it can be manually set.
        /// </summary>
        public AuthorizationInfo AuthorizationInfo { get; private set; } = new AuthorizationInfo();

        /// <summary>
        /// The OAuth 2.0 token endpoint of the Q REST API.
        /// </summary>
        public const string CloudAuthenticationEndPoint = "https://q.daskeyboard.com/oauth/1.4/token";

        /// <summary>
        /// The base Q REST API endpoint. This endpoint requires
        /// authentication.
        /// </summary>
        public const string CloudEndPoint = "https://q.daskeyboard.com/api/1.0/";

        /// <summary>
        /// The base local API endpoint. This endpoint does not require
        /// authentication.
        /// </summary>
        public const string LocalEndPoint = "http://localhost:27301/api/1.0/";

        /// <summary>
        /// The OAuth 2.0 token endpoint to use for authentication. The default
        /// value is defined by <see cref="CloudAuthenticationEndPoint"/>.
        /// </summary>
        public string AuthenticationEndPoint { get; set; } = CloudAuthenticationEndPoint;

        /// <summary>
        /// The base API endpoint. The default value is defined by <see
        /// cref="CloudEndPoint"/>. The value defined by <see
        /// cref="LocalEndPoint"/> may also be used.
        /// </summary>
        public string ApiEndPoint { get; set; } = CloudEndPoint;

        /// <summary>
        /// The OAuth 2.0 grant type to use. The default value is <see
        /// cref="AuthenticationMode.ClientCredentials"/>.
        /// </summary>
        public AuthenticationMode AuthenticationMode { get; set; } = AuthenticationMode.ClientCredentials;

        /// <summary>
        /// The credentials to use for authentication. The values for <see
        /// cref="NetworkCredential.UserName"/> and <see
        /// cref="NetworkCredential.Password"/> are different depending on the
        /// value of <see cref="AuthenticationMode"/>.
        /// </summary>
        /// <remarks>
        /// <table>
        /// <tr><th>AuthenticationMode</th><th>UserName</th><th>Password</th></tr>
        /// <tr><td><see cref="AuthenticationMode.ClientCredentials"/></td><td>The OAuth 2.0 client identifier</td><td>The OAuth 2.0 client password</td></tr>
        /// <tr><td><see cref="AuthenticationMode.Password"/></td><td>The q.daskeyboard.com account email address</td><td>The q.daskeyboard.com account password</td></tr>
        /// <tr><td><see cref="AuthenticationMode.AuthorizationCode"/></td><td>The OAuth 2.0 client identifier</td><td>The OAuth 2.0 authorization code</td></tr>
        /// <tr><td><see cref="AuthenticationMode.RefreshToken"/></td><td>The OAuth 2.0 client identifier</td><td>The OAuth 2.0 refresh token</td></tr>
        /// <tr><td><see cref="AuthenticationMode.None"/></td><td>Not used</td><td>Not used</td></tr>
        /// </table>
        /// <para>
        /// If the Q REST API issues an OAuth 2.0 refresh token, then these
        /// credentials are only used to retreive the initial access token and
        /// refresh token, and the refresh token is used to retreive subsequent
        /// access tokens. Otherwise, these credentials are used before each
        /// API request.
        /// </para>
        /// </remarks>
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Instantiates a new instance of a <see cref="Client"/>.
        /// </summary>
        public Client()
        {
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private Uri ResolveApiUri(string resource)
        {
            return new Uri(new Uri(this.ApiEndPoint), resource);
        }

        private async Task Authenticate()
        {
            if (this.AuthenticationMode == AuthenticationMode.None)
            {
                this.httpClient.DefaultRequestHeaders.Authorization = null;
                return;
            }

            if (this.AuthorizationInfo.RefreshToken != null && this.AuthorizationInfo.ExpiresIn.HasValue && DateTime.Now > this.lastRefreshTime.AddSeconds(this.AuthorizationInfo.ExpiresIn.Value))
            {
                var credentials = new RefreshTokenCredentials { ClientId = this.Credentials.UserName, RefreshToken = this.AuthorizationInfo.RefreshToken };
                var requestSerializer = new DataContractJsonSerializer(credentials.GetType());
                var requestStream = new MemoryStream();

                requestSerializer.WriteObject(requestStream, credentials);

                var content = new StreamContent(requestStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await this.httpClient.PostAsync(this.AuthenticationEndPoint, content);

                var responseSerializer = new DataContractJsonSerializer(typeof(AuthorizationInfo));
                var authorizationInfo = responseSerializer.ReadObject(await response.Content.ReadAsStreamAsync()) as AuthorizationInfo;
                this.AuthorizationInfo.Update(authorizationInfo);
                this.lastRefreshTime = DateTime.Now;
            }
            else if (this.AuthorizationInfo == null || this.AuthorizationInfo.AccessToken == null)
            {
                object credentials = null;

                switch (this.AuthenticationMode)
                {
                    case AuthenticationMode.ClientCredentials:
                        credentials = new ClientCredentials { ClientId = this.Credentials.UserName, ClientSecret = this.Credentials.Password };
                        break;

                    case AuthenticationMode.Password:
                        credentials = new PasswordCredentials { Email = this.Credentials.UserName, Password = this.Credentials.Password };
                        break;

                    case AuthenticationMode.AuthorizationCode:
                        credentials = new AuthorizationCodeCredentials { ClientId = this.Credentials.UserName, Code = this.Credentials.Password };
                        break;

                    case AuthenticationMode.RefreshToken:
                        credentials = new RefreshTokenCredentials { ClientId = this.Credentials.UserName, RefreshToken = this.Credentials.Password };
                        break;
                }

                var requestSerializer = new DataContractJsonSerializer(credentials.GetType());
                var requestStream = new MemoryStream();

                requestSerializer.WriteObject(requestStream, credentials);
                requestStream.Position = 0;

                var content = new StreamContent(requestStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await this.httpClient.PostAsync(this.AuthenticationEndPoint, content);

                var responseSerializer = new DataContractJsonSerializer(typeof(AuthorizationInfo));
                var authorizationInfo = responseSerializer.ReadObject(await response.Content.ReadAsStreamAsync()) as AuthorizationInfo;
                this.AuthorizationInfo.Update(authorizationInfo);
                this.lastRefreshTime = DateTime.Now;
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthorizationInfo.AccessToken);
        }

        private async Task<T> Get<T>(string resource) where T : class
        {
            await this.Authenticate();
            var serializer = new DataContractJsonSerializer(typeof(T));
            var stream = await this.httpClient.GetStreamAsync(this.ResolveApiUri(resource));
            return serializer.ReadObject(stream) as T;
        }

        private async Task Post(string resource, object data)
        {
            await this.Authenticate();
            var serializer = new DataContractJsonSerializer(data.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, data);
            stream.Position = 0;
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await this.httpClient.PostAsync(this.ResolveApiUri(resource), content);
        }

        private async Task<T> Post<T>(string resource, object data) where T : class
        {
            await this.Authenticate();
            var requestSerializer = new DataContractJsonSerializer(data.GetType());
            var requestStream = new MemoryStream();
            requestSerializer.WriteObject(requestStream, data);
            requestStream.Position = 0;
            var content = new StreamContent(requestStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await this.httpClient.PostAsync(this.ResolveApiUri(resource), content);
            var responseSerializer = new DataContractJsonSerializer(typeof(T));
            return responseSerializer.ReadObject(await response.Content.ReadAsStreamAsync()) as T;
        }

        private async Task<T> Patch<T>(string resource, object data) where T : class
        {
            await this.Authenticate();
            var requestSerializer = new DataContractJsonSerializer(data.GetType());
            var requestStream = new MemoryStream();
            requestSerializer.WriteObject(requestStream, data);
            requestStream.Position = 0;
            var content = new StreamContent(requestStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), this.ResolveApiUri(resource)) { Content = content };

            var response = await this.httpClient.SendAsync(request);
            var responseSerializer = new DataContractJsonSerializer(typeof(T));
            return responseSerializer.ReadObject(await response.Content.ReadAsStreamAsync()) as T;
        }

        private async Task Delete(string resource)
        {
            await this.Authenticate();
            await this.httpClient.DeleteAsync(this.ResolveApiUri(resource));
        }

        /// <summary>
        /// Gets the names of authorized clients.
        /// </summary>
        public async Task<IEnumerable<string>> GetAuthorizedClients()
        {
            return (await this.Get<IEnumerable<AuthorizedClient>>("users/authorized_clients")).Select(c => c.Name);
        }

        /// <summary>
        /// Revokes the speficied authorized client.
        /// </summary>
        /// <param name="name">The name of the authorized client to revoke.</param>
        public Task RevokeAuthorizedClient(string name)
        {
            return this.Post("users/revoke_client", new AuthorizedClient { Name = name });
        }

        /// <summary>
        /// Gets the devices associated with this account.
        /// </summary>
        public Task<IEnumerable<Device>> GetDevices()
        {
            return this.Get<IEnumerable<Device>>("devices");
        }

        /// <summary>
        /// Gets the zones associated with a specified device.
        /// </summary>
        /// <param name="pid">The PID of the device.</param>
        public Task<IEnumerable<Zone>> GetZones(string pid)
        {
            return this.Get<IEnumerable<Zone>>($"{pid}/zones");
        }

        /// <summary>
        /// Creates a signal.
        /// </summary>
        /// <param name="signal">The signal to create.</param>
        public async Task CreateSignal(Signal signal)
        {
            signal.Update(await this.Post<Signal>("signals", signal));
        }

        //public Task<IEnumerable<Signal>> GetSignals()
        //{
        //    return this.Get<IEnumerable<Signal>>("signals");
        //}

        /// <summary>
        /// Updates a signal.
        /// </summary>
        /// <param name="signal">The signal to update.</param>
        /// <remarks>
        /// Only the <see cref="Signal.Id"/>, <see cref="Signal.IsArchived"/>,
        /// <see cref="Signal.IsMuted"/>, and <see cref="Signal.IsRead"/>
        /// properties are used to update the signal.
        /// </remarks>
        public async Task UpdateSignal(Signal signal)
        {
            var data = new Signal
            {
                IsMuted = signal.IsMuted,
                IsRead = signal.IsRead,
                IsArchived = signal.IsArchived,
            };

            signal.Update(await this.Patch<Signal>($"signals/{signal.Id}/status", data));
        }

        /// <summary>
        /// Deletes a signal.
        /// </summary>
        /// <param name="signal">The signal to delete.</param>
        /// <remarks>
        /// Only the <see cref="Signal.Id"/> property of the signal is used to
        /// delete the signal.
        /// </remarks>
        public Task DeleteSignal(Signal signal)
        {
            return this.DeleteSignal(signal.Id.Value);
        }

        /// <summary>
        /// Deletes a signal.
        /// </summary>
        /// <param name="id">The ID of the signal to delete.</param>
        public Task DeleteSignal(int id)
        {
            return this.Delete($"signals/{id}");
        }
    }
}
