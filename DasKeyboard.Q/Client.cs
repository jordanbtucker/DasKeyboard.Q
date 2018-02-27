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
    public class Client
    {
        private HttpClient httpClient = new HttpClient();

        private AuthorizationInfo authorizationInfo;

        public const string CloudAuthenticationEndPoint = "https://q.daskeyboard.com/oauth/1.4/token";

        public const string CloudEndPoint = "https://q.daskeyboard.com/api/1.0/";

        public const string LocalEndPoint = "http://localhost:27301/api/1.0/";

        public string AuthenticationEndPoint { get; set; } = CloudAuthenticationEndPoint;

        public string ApiEndPoint { get; set; } = CloudEndPoint;

        public AuthenticationMode AuthenticationMode { get; set; } = AuthenticationMode.ClientCredentials;

        public NetworkCredential Credentials { get; set; }

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

            if (this.authorizationInfo == null || this.authorizationInfo.AccessToken == null)
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
                }

                var requestSerializer = new DataContractJsonSerializer(credentials.GetType());
                var requestStream = new MemoryStream();

                requestSerializer.WriteObject(requestStream, credentials);

                var content = new StreamContent(requestStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await this.httpClient.PostAsync(this.AuthenticationEndPoint, content);

                var responseSerializer = new DataContractJsonSerializer(typeof(AuthorizationInfo));
                this.authorizationInfo = responseSerializer.ReadObject(await response.Content.ReadAsStreamAsync()) as AuthorizationInfo;
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.authorizationInfo.AccessToken);
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

        public async Task<IEnumerable<string>> GetAuthorizedClients()
        {
            return (await this.Get<IEnumerable<AuthorizedClient>>("users/authorized_clients")).Select(c => c.Name);
        }

        public Task RevokeAuthorizedClient(string name)
        {
            return this.Post("users/revoke_client", new AuthorizedClient { Name = name });
        }

        public Task<IEnumerable<Device>> GetDevices()
        {
            return this.Get<IEnumerable<Device>>("devices");
        }

        public Task<IEnumerable<Zone>> GetZones(string pid)
        {
            return this.Get<IEnumerable<Zone>>($"{pid}/zones");
        }

        public async Task CreateSignal(Signal signal)
        {
            signal.Update(await this.Post<Signal>("signals", signal));
        }

        //public Task<IEnumerable<Signal>> GetSignals()
        //{
        //    return this.Get<IEnumerable<Signal>>("signals");
        //}

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

        public Task DeleteSignal(Signal signal)
        {
            return this.Delete($"signals/{signal.Id}");
        }
    }
}
