using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    class PasswordCredentials
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "grant_type")]
        public string GrantType = "password";
    }
}
