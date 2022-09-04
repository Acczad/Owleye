using MediatR;
using Owleye.Application.Dto;
using Owleye.Domain;
using System;
using System.Collections.Generic;

namespace Owleye.Application.Notifications.Messages
{
    public class PageLoadResultNotification : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public Dictionary<NotificationType, List<string>> NotificationList { get; set; }
        public bool LoadSuccess { get; set; }
        public DateTime LastAvilable { get; set; }
    }

}
