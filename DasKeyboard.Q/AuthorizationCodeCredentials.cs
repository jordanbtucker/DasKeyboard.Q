using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    class AuthorizationCodeCredentials
    {
        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "grant_type")]
        public string GrantType = "code";
    }
}
