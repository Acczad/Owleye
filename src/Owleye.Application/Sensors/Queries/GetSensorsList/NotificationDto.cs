using Owleye.Domain;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class NotificationDto
    {
        public int EndPointId { get; set; }
        public string NoTificationAddress { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
