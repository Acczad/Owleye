using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class NotificationDto
    {
        public int EndPointId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

    }
}
