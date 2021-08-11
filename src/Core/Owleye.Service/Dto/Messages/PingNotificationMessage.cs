﻿using MediatR;
using System.Collections.Generic;

namespace Owleye.Core.Notifications.Messages
{
    public class PingNotificationMessage: INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public List<string> EmailNotify { get; set; }
        public List<string> MobileNotify { get; set; }
        public bool  PingSuccess { get; set; }
    }
}
