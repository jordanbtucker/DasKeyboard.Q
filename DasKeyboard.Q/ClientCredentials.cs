using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    class ClientCredentials
    {
        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "client_secret")]
        public string ClientSecret { get; set; }

        [DataMember(Name = "grant_type")]
        public string GrantType = "client_credentials";
    }
}
