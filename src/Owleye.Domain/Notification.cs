using Owleye.Shared.Data;
using System;

namespace Owleye.Domain
{
    [Serializable]
    public class Notification : BaseEntity
    {
        public int EndPointId { get; set; }
        public string NoTificationAddress { get; set; }
        public NotificationType NotificationType { get; set; }

        public EndPoint EndPoint { get; set; }
    }

    public enum NotificationType
    {
        Sms = 1,
        Email = 2,
        Discord = 3,
        Console = 4,
        MicrosoftTeam = 5
    }
}
