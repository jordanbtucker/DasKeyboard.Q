using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    public class Zone
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
