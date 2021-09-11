using MediatR;
using System.Collections.Generic;

namespace Owleye.Application.Dto.Messages
{
    public class DoPageLoadNotification : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public List<string> EmailNotify { get; set; }
        public List<string> MobileNotify { get; set; }
    }
}
