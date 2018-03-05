using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    public class AuthorizationInfo
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "user_id")]
        public int? UserId { get; set; }

        [DataMember(Name = "expires_in")]
        public int? ExpiresIn { get; set; }

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
