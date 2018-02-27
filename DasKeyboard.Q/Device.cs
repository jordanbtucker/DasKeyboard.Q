using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    public class Device
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [DataMember(Name = "pid")]
        public string Pid { get; set; }

        [DataMember(Name = "firmwareVersion")]
        public string FirmwareVersion { get; set; }

        [DataMember(Name = "vid")]
        public string Vid { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "zones")]
        public IEnumerable<Zone> Zones { get; set; }
    }
}
