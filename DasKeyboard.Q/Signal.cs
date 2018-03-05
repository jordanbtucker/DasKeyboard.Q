using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    /// <summary>
    /// Represents a Q REST API signal.
    /// </summary>
    [DataContract]
    public class Signal
    {
        /// <summary>
        /// The ID of the signal.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The zone ID of the signal.
        /// </summary>
        [DataMember(Name = "zoneId")]
        public string ZoneId { get; set; }

        /// <summary>
        /// The PID of the device associated with the signal.
        /// </summary>
        [DataMember(Name = "pid")]
        public string Pid { get; set; }

        /// <summary>
        /// The ID of the q.daskeybaord.com account associated with the signal.
        /// </summary>
        [DataMember(Name = "userId")]
        public int? UserId { get; set; }

        /// <summary>
        /// The name of the signal.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The message associated with the signal.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// The effect of the signal.
        /// </summary>
        [DataMember(Name = "effect")]
        public string Effect { get; set; }

        /// <summary>
        /// The color of the signal.
        /// </summary>
        [DataMember(Name = "color")]
        public string Color { get; set; }

        /// <summary>
        /// A value indicating whether the signal is read.
        /// </summary>
        [DataMember(Name = "isRead")]
        public bool? IsRead { get; set; }

        /// <summary>
        /// A value indicating whether the signal is muted.
        /// </summary>
        [DataMember(Name = "isMuted")]
        public bool? IsMuted { get; set; }

        /// <summary>
        /// A value indicating whether the signal is archived.
        /// </summary>
        [DataMember(Name = "isArchived")]
        public bool? IsArchived { get; set; }

        /// <summary>
        /// A value indicating whether the signal should produce a
        /// notification.
        /// </summary>
        [DataMember(Name = "shouldNotify")]
        public bool? ShouldNotify { get; set; }

        /// <summary>
        /// The name of the client associated with the signal.
        /// </summary>
        [DataMember(Name = "clientName")]
        public string ClientName { get; set; }

        /// <summary>
        /// The Unix timestamp of when the signal was read.
        /// </summary>
        [DataMember(Name = "readAt")]
        public int? ReadAt { get; set; }

        /// <summary>
        /// The action of the signal.
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; set; }

        /// <summary>
        /// The Unix timestamp of when the signal was created.
        /// </summary>
        [DataMember(Name = "createdAt")]
        public int? CreatedAt { get; set; }

        /// <summary>
        /// The Unix timestamp of when the signal was last updated.
        /// </summary>
        [DataMember(Name = "updatedAt")]
        public int? UpdatedAt { get; set; }

        internal void Update(Signal signal)
        {
            this.Id = signal.Id;
            this.ZoneId = signal.ZoneId;
            this.Pid = signal.Pid;
            this.UserId = signal.UserId;
            this.Name = signal.Name;
            this.Message = signal.Message;
            this.Effect = signal.Effect;
            this.Color = signal.Color;
            this.IsRead = signal.IsRead;
            this.IsMuted = signal.IsMuted;
            this.IsArchived = signal.IsArchived;
            this.ShouldNotify = signal.ShouldNotify;
            this.ClientName = signal.ClientName;
            this.ReadAt = signal.ReadAt;
            this.Action = signal.Action;
            this.CreatedAt = signal.CreatedAt;
            this.UpdatedAt = signal.UpdatedAt;
        }
    }
}
