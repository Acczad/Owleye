using MediatR;
using System.Collections.Generic;

namespace Owleye.Application.Dto.Messages
{
    public class DoPingMessage : INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public List<string> EmailNotify { get; set; }
        public List<string> MobileNotify { get; set; }
    }
}
