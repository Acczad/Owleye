using MediatR;
using Owleye.Domain;
using System.Collections.Generic;

namespace Owleye.Application.Dto.Messages
{
    public class DoPageLoadNotification : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public string Name { get; set; }
        public Dictionary<NotificationType, List<string>> NotificationList { get; set; }
    }
}
