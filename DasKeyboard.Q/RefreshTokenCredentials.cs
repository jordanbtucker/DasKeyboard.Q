using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    class RefreshTokenCredentials
    {
        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "grant_type")]
        public string GrantType = "refresh_token";
    }
}
