using MediatR;
using Owleye.Domain;
using System.Collections.Generic;

namespace Owleye.Application.Notifications.Messages
{
    public class PingResultNotification: INotification
    {
        public int EndPointId { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
       public Dictionary<NotificationType, List<string>> NotificationList { get; set; }
        public bool  PingSuccess { get; set; }
    }
}
