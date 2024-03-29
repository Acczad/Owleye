﻿using MediatR;
using Owleye.Domain;
using System;
using System.Collections.Generic;

namespace Owleye.Application.Notifications.Messages
{
    public class NotifyViaMicrosoftTeamNotification : INotification
    {
        public string ServiceUrl { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public SensorType SensorType { get; set; }
        public bool IsServiceAlive { get; set; }
        public DateTime? LastAvailable { get; set; }
        public List<string> EmailAddresses { get; set; }
    }
}
