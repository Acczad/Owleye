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
    public class GetSensorsListQueryHandler : IRequestHandler<GetSensorsListPagedQuery, QueryPagedResult<SensorDto>>
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
        async Task<QueryPagedResult<SensorDto>> IRequestHandler<GetSensorsListPagedQuery, QueryPagedResult<SensorDto>>.Handle(GetSensorsListPagedQuery request, CancellationToken cancellationToken)
        {
            var result = (await _sensorRepository
                .GetPagedAsync(new PagedQuery(request.PageSize, request.PageNumber)));

            var mapped = _mapper.Map<List<Sensor>, List<SensorDto>>(result.Data.ToList());

            var rs = new QueryPagedResult<SensorDto>
            {
                Data=mapped,
                Page=request.PageNumber,
                PageCount= result.PageCount,
                PageSize=result.PageSize
            };
                
            return rs;

        }
    }
}
