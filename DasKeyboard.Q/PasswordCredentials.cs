using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    class PasswordCredentials
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
