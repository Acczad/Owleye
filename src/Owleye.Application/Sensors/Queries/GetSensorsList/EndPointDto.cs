using System.Collections.Generic;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class EndPointDto
    {
        public string Name { get; protected set; }
        public string IpAddress { get; protected set; }
        public string Url { get; protected set; }
        public string WebPageMetaKeyword { get; protected set; }
        public List<NotificationDto> Notification { get; set; }
    }
}
