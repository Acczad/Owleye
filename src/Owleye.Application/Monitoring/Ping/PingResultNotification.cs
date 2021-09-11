using MediatR;
using System.Collections.Generic;

namespace Owleye.Application.Notifications.Messages
{
    public class PingResultNotification: INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public List<string> EmailNotify { get; set; }
        public List<string> MobileNotify { get; set; }
        public bool  PingSuccess { get; set; }
    }
}
