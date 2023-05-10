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
    public class DoPortCheckNotificationHandler : INotificationHandler<DoPortCheckNotification>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly IConfiguration _configuration;

        public DoPortCheckNotificationHandler(
            IMediator mediator,
            IRedisCache cache,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _cache = cache;
            _configuration = configuration;
        }


        public async Task Handle(DoPortCheckNotification notification, CancellationToken cancellationToken)
        {
            var cacheKey = $"{notification.EndPointId}-{nameof(SensorType.PortCheck)}";
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

            var portCheckResult = PortUtil.IsPortAlive(notification.IpAddress, 10);

            if (!portCheckResult) // IS Network availability
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
                await _mediator.Publish(new PortCheckResultNotification
                {
                    IpAddress = notification.IpAddress,
                    EndPointId = notification.EndPointId,
                    NotificationList=notification.NotificationList,
                    PortCheckSuccess = portCheckResult
                }, cancellationToken);

            }
        }
    }
}
