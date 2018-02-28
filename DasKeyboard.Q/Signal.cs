using System.Runtime.Serialization;

namespace DasKeyboard.Q
{
    [DataContract]
    public class Signal
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "zoneId")]
        public string ZoneId { get; set; }

        [DataMember(Name = "pid")]
        public string Pid { get; set; }

        [DataMember(Name = "userId")]
        public int? UserId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "effect")]
        public string Effect { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        [DataMember(Name = "isRead")]
        public bool? IsRead { get; set; }

        [DataMember(Name = "isMuted")]
        public bool? IsMuted { get; set; }

        [DataMember(Name = "isArchived")]
        public bool? IsArchived { get; set; }

        [DataMember(Name = "shouldNotify")]
        public bool? ShouldNotify { get; set; }

        [DataMember(Name = "clientName")]
        public string ClientName { get; set; }

        [DataMember(Name = "readAt")]
        public int? ReadAt { get; set; }

        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "createdAt")]
        public int? CreatedAt { get; set; }

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
