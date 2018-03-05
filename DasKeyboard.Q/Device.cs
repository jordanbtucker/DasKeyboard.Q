using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    /// <summary>
    /// Represents a Q REST API keyboard device.
    /// </summary>
    [DataContract]
    public class Device
    {
        /// <summary>
        /// The ID of the device.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// The ID of the q.daskeybaord.com account associated with the device.
        /// </summary>
        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        /// <summary>
        /// The PID of the device.
        /// </summary>
        [DataMember(Name = "pid")]
        public string Pid { get; set; }

        /// <summary>
        /// The firmware version of the device.
        /// </summary>
        [DataMember(Name = "firmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// The VID of the device.
        /// </summary>
        [DataMember(Name = "vid")]
        public string Vid { get; set; }

        /// <summary>
        /// A description of the device.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// The zones associated with the device.
        /// </summary>
        [DataMember(Name = "zones")]
        public IEnumerable<Zone> Zones { get; set; }
    }
}
