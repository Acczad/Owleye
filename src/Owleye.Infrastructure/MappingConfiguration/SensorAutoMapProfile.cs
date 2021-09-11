using AutoMapper;
using Owleye.Domain;
using Owleye.Application.Dto;
using Owleye.Application.Sensors.Queries.GetSensorsList;

namespace Owleye.Infrastructure.MappingConfiguration
{
    public class SensorAutoMapProfile : Profile
    {
        public SensorAutoMapProfile()
        {
            CreateMap<Sensor, SensorDto>();
            CreateMap<EndPoint, EndPointDto>();
            CreateMap<Notification, NotificationDto>();
        }
    }
}
