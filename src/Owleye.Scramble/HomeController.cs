using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Owleye.Application.Dto.Messages;
using Owleye.Shared.Cache;
using System.Linq;
using System.Threading.Tasks;

namespace Owleye.Scramble
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRedisCache redisCache;
        private readonly IMediator mediator;

        public HomeController(
            IConfiguration configuration,
            IRedisCache redisCache,
            IMediator mediator)
        {
            this.configuration = configuration;
            this.redisCache = redisCache;
            this.mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            var url = configuration["DomainUrl"];
            var emailList = configuration["EmmailNotify"].Split(';').ToList();

            if (Shared.Util.WebSiteUtil.IsUrlAlive(url, 10000))
                return Redirect(url);

            return View();
        }
    }
}
