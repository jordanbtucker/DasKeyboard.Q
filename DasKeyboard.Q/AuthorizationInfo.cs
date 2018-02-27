using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    class AuthorizationInfo
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "user_id")]
        public int UserId { get; set; }
    }
}
