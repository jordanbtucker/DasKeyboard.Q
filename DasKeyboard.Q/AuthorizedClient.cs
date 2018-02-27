using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    class AuthorizedClient
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
