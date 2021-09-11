using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Owleye.Shared.Cache;
using Owleye.Shared.Util;
using Owleye.Application.Dto;
using Owleye.Application.Dto.Messages;
using Owleye.Application.Notifications.Messages;
using Owleye.Domain;

namespace Owleye.Application.Handlers
{
    public class DoPageLoadHandler : INotificationHandler<DoPageLoadMessage>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly IConfiguration _configuration;

        public DoPageLoadHandler(
            IMediator mediator,
            IRedisCache cache,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _cache = cache;
            _configuration = configuration;
        }


        public async Task Handle(DoPageLoadMessage notification, CancellationToken cancellationToken)
        {
            var cacheKey = $"{notification.EndPointId}-{nameof(SensorType.PageLoad)}";
            var operation = await _cache.GetAsync<OngoingOperationDto>(cacheKey);


            if (operation != null)
            {
                if ((DateTime.Now - operation.StartDate).TotalMinutes <= 1)
                    return;
            }
            else
            {
                await _cache.SetAsync(cacheKey, new OngoingOperationDto(DateTime.Now));
            }

            var networkavailability = true;

            var urlLoadTimeout = int.Parse(_configuration["General:UrlLoadTimeout"]);
            var urlResult = WebSiteUtil.IsUrlAlive(notification.PageUrl, urlLoadTimeout);

            if (urlResult == false) // check network availability
            {
                var pingAddress = _configuration["General:PingAddress"];
                networkavailability = PingUtil.Ping(pingAddress);
            }

            if (networkavailability == false)
            {
                //TODO Notify about  connection
            }
            else
            {
                await _mediator.Publish(new PageLoadNotificationMessage
                {
                    PageUrl = notification.PageUrl,
                    EmailNotify = notification.EmailNotify,
                    EndPointId = notification.EndPointId,
                    MobileNotify = notification.MobileNotify,
                    LoadSuccess = urlResult
                }, cancellationToken);

            }
        }
    }
}
