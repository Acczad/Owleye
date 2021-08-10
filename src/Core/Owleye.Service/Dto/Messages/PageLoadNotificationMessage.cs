﻿using MediatR;
using System;
using System.Collections.Generic;

namespace Owleye.Service.Notifications.Messages
{
    public class PageLoadNotificationMessage : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public List<string> EmailNotify { get; set; }
        public List<string> MobileNotify { get; set; }
        public bool LoadSuccess { get; set; }
        public DateTime LastAvilable { get; set; }
    }
}
