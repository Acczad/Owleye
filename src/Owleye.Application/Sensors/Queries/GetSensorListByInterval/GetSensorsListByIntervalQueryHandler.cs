using AutoMapper;
using MediatR;
using Owleye.Domain;
using Owleye.Application.Dto;
using Owleye.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Owleye.Shared.Model;

namespace Owleye.Application.Sensors.Queries.GetSensorsList
{
    public class GetSensorsListByIntervalQueryHandler : IRequestHandler<GetSensorsListByIntervalQuery, QueryPagedResult<SensorDto>>
    {
        private readonly IGenericRepository<Sensor> _sensorRepository;
        private readonly IMapper _mapper;

        public GetSensorsListByIntervalQueryHandler(
            IGenericRepository<Sensor> sensorRepository,
            IMapper mapper
            )
        {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
        }

        public async Task<QueryPagedResult<SensorDto>> Handle(GetSensorsListByIntervalQuery request, CancellationToken cancellationToken)
        {
            var includeProperties = new Expression<Func<Sensor, dynamic>>[2];
            includeProperties[0] = i => i.EndPoint;
            includeProperties[1] = i => i.EndPoint.Notification;

            var endPointList = (await _sensorRepository
                .GetAsync(filter: q => q.SensorInterval == request.SensorInterval,
                    includeProperties: includeProperties)).ToList();

            if (endPointList==null || endPointList.Count < 0)
            {
                return new QueryPagedResult<SensorDto>();
            }

            var result = _mapper.Map<List<Sensor>, List<SensorDto>>(endPointList);

            return new QueryPagedResult<SensorDto>(result);
        }
    }
}
