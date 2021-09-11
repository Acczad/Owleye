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

        public static EndPoint Create(string name, string ipAddress, string url, string webPageMetaKeywords)
        {
            name = name.Trim();
            webPageMetaKeywords = webPageMetaKeywords.Trim();

            Guard.Argument(name, nameof(name)).NotEmpty().NotNull();

            if (ipAddress.IsNotNullOrEmpty())
            {
                var isValid = ipAddress.IsIPv4();
                Guard.Argument(isValid, nameof(ipAddress)).True("Invalid ip address");
            }

            if (url.IsNotNullOrEmpty())
            {
                var isValid = url.IsUrl();
                Guard.Argument(isValid, nameof(url)).True("Invalid url address");
            }

            return new EndPoint
            {
                Name = name,
                IpAddress = ipAddress,
                Created = DateTimeOffset.Now,
                Url = url,
                WebPageMetaKeyword = webPageMetaKeywords,
            };

        }

        public ICollection<Notification> Notification { get; set; }
        public ICollection<Sensor> Sensors { get; set; }

    }

}


