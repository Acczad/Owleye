using Owleye.Domain;
using Owleye.Application.Sensors.Queries.GetSensorsList;

namespace Owleye.Application.Dto
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EndPointId { get; set; }
        public SensorType SensorType { get; set; }
        public SensorInterval SensorInterval { get; set; }
        public EndPointDto EndPoint{ get; set; }
        
    }
}
