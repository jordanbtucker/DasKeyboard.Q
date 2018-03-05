using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    /// <summary>
    /// Represents a Q REST API zone.
    /// </summary>
    [DataContract]
    public class Zone
    {
        /// <summary>
        /// The ID of the zone.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// A description of the zone.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the zone.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
