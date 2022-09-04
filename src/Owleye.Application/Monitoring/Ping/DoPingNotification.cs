using MediatR;
using Owleye.Domain;
using System.Collections.Generic;

namespace Owleye.Application.Dto.Messages
{
    public class DoPingNotification : INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public Dictionary<NotificationType,List<string>> NotificationList { get; set; }
    }
}
