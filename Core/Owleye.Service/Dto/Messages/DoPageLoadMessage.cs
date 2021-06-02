﻿using MediatR;
using System.Collections.Generic;

namespace Owleye.Service.Dto.Messages
{
    public class DoPageLoadMessage : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public List<string> EmailNotify { get; set; }
        public List<string> MobileNotify { get; set; }
    }
}
