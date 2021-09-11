using MediatR;
using Owleye.Application.Dto;
using Owleye.Shared.Model;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class GetSensorsListQuery: IRequest<QueryListResult<SensorDto>>
    {

    }
}
