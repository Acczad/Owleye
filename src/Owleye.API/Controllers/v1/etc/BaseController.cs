using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Owleye.Shared.Base;
using System.Linq;
using System.Security.Claims;

namespace Owleye.API.Owleye.API.v1
{
    public class BaseController : Controller
    {
        private readonly IAppSession _appSession;
        
        public BaseController(IAppSession appSession, IHttpContextAccessor httpContextAccessor)
        {
            _appSession = appSession;
            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimEmail = identity.Claims.FirstOrDefault(q => q.Type == "Email");
                var claimId = identity.Claims.FirstOrDefault(q => q.Type == "Id");

                if (claimEmail != null)
                {
                    _appSession.EmailAddress = claimEmail.Value;
                };

                if (claimId != null)
                {
                    _appSession.Id = int.Parse(claimId.Value);
                };

            }
        }
    }
}
