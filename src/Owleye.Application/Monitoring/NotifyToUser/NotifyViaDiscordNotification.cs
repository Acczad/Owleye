using MediatR;
using Owleye.Domain;
using System;
using System.Collections.Generic;

namespace Owleye.Application.Notifications.Messages
{
    public class NotifyViaDiscordNotification : INotification
    {
        public string ServiceUrl { get; set; }
        public string IpAddress { get; set; }
        public SensorType SensorType { get; set; }
        public List<string> DiscordHookApis { get; set; }
        public bool IsServiceAlive { get; set; }
        public DateTime? LastAvailable { get; set; }
        public string Name { get; set; }
    }
}
