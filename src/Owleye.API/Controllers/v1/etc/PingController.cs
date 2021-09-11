using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Owleye.API.Owleye.API.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PingController : ControllerBase
    {

        [HttpGet]
        [SwaggerOperation(
            Summary = "Ping endpoint for availability status ",
            Description = "api availability check",
            OperationId = "Ping.Get",
            Tags = new[] { "HealthCheckEndpoints" })
        ]
        public IActionResult Get()
        {
            return Ok("v1 :)");
        }
    }
}
