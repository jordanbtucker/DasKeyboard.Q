using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    class AuthorizedClient
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
