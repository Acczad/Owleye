﻿using MediatR;
using Owleye.Domain;
using System.Collections.Generic;

namespace Owleye.Application.Notifications.Messages
{
    public class PortCheckResultNotification : INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public Dictionary<NotificationType, List<string>> NotificationList { get; set; }
        public bool PortCheckSuccess { get; set; }
    }
}
