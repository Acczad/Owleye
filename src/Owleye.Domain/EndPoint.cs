using System;
using System.Collections.Generic;
using Dawn;
using Extension.Methods;
using Owleye.Shared.Data;

namespace Owleye.Domain
{
    [Serializable]
    public class EndPoint : BaseEntity
    {
        protected EndPoint()
        {
        }

        public string Name { get; protected set; }
        public string IpAddress { get; protected set; }
        public string Url { get; protected set; }
        public string WebPageMetaKeyword { get; protected set; }

        public ICollection<Notification> Notification { get; set; }
        public ICollection<Sensor> Sensors { get; set; }

    }

}


