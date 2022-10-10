using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Owleye.Scramble
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var url = configuration["DomainUrl"];
            var domainName = configuration["DomainName"];
            var companyName = configuration["CompanyName"];
            var companyUrl = configuration["CompanyUrl"];

            ViewData["CompanyName"] = companyName;
            ViewData["CompanyUrl"] = companyUrl;
            ViewData["DomainUrl"] = url;
            ViewData["DomainName"] = domainName;

            var siteStatus = Shared.Util.WebSiteUtil.IsUrlAliveWithStatus(url, 10000);
            if (siteStatus.Item1)
                return Redirect(url);

            ViewData["ErroCode"] = siteStatus.Item2;

            return View();
        }
    }
}
