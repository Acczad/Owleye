using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Owleye.Application.Sensors.Queries.GetSensorsList;
using Owleye.Shared.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Owleye.API.Owleye.API.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SensorController : BaseController
    {
        private readonly IMediator _mediator;

        public SensorController(IAppSession appSession,
                                IHttpContextAccessor httpContextAccessor,
                                IMediator mediator) : base(appSession, httpContextAccessor)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Sensor list endpoint",
            Description = "Sensor list endpoint",
            OperationId = "",
            Tags = new[] { "sensor list" })
        ]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var query = new GetSensorsListQuery { };

            var result = await _mediator.Send(query);

            return Ok(result);

        }



    }
}
