using MediatR;
using Owleye.Domain;
using Owleye.Application.Dto;
using Owleye.Shared.Model;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class GetSensorsListByIntervalQuery : IRequest<QueryPagedResult<SensorDto>>
    {
        public SensorInterval SensorInterval { get; set; }
    }
}
