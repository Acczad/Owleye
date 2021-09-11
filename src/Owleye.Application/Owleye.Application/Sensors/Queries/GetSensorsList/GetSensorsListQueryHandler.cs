using AutoMapper;
using MediatR;
using Owleye.Domain;
using Owleye.Application.Dto;
using Owleye.Shared.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Owleye.Shared.Model;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class GetSensorsListQueryHandler : IRequestHandler<GetSensorsListQuery, QueryListResult<SensorDto>>
    {
        private readonly IGenericRepository<Sensor> _sensorRepository;
        private readonly IMapper _mapper;

        public GetSensorsListQueryHandler(
            IGenericRepository<Sensor> sensorRepository,
            IMapper mapper
            )
        {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
        }
        async Task<QueryListResult<SensorDto>> IRequestHandler<GetSensorsListQuery, QueryListResult<SensorDto>>.Handle(GetSensorsListQuery request, CancellationToken cancellationToken)
        {
            var result = (await _sensorRepository.GetAsync()).ToList();

            var mapped = _mapper.Map<List<Sensor>, List<SensorDto>>(result);

            var rs = new QueryListResult<SensorDto>(mapped);

            return rs;

        }
    }
}
