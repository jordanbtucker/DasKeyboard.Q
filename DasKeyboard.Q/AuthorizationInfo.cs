using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    /// <summary>
    /// Represents the authorization information used to authenticate with the
    /// Q REST API.
    /// </summary>
    [DataContract]
    public class AuthorizationInfo
    {
        /// <summary>
        /// The OAuth 2.0 access token.
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The OAuth 2.0 refresh token.
        /// </summary>
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// The q.daskeyboard.com account user ID.
        /// </summary>
        [DataMember(Name = "user_id")]
        public int? UserId { get; set; }

        /// <summary>
        /// The expiration time of <see cref="AccessToken"/> in number of
        /// seconds since issuance.
        /// </summary>
        [DataMember(Name = "expires_in")]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Updates the authorization information.
        /// </summary>
        /// <param name="authorizationInfo">The authorization information to update.</param>
        public void Update(AuthorizationInfo authorizationInfo)
        {
            if (authorizationInfo.AccessToken != null)
                this.AccessToken = authorizationInfo.AccessToken;

            if (authorizationInfo.RefreshToken != null)
                this.RefreshToken = authorizationInfo.RefreshToken;

            if (authorizationInfo.UserId.HasValue)
                this.UserId = authorizationInfo.UserId;
        }
    }
}
