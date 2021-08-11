﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Owleye.Shared.Cache;
using Owleye.Shared.Util;
using Owleye.Core.Dto;
using Owleye.Core.Dto.Messages;
using Owleye.Core.Notifications.Messages;
using Owleye.Core.Aggrigate;

namespace Owleye.Core.Handlers
{
    public class DoPingHandler : INotificationHandler<DoPingMessage>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly IConfiguration _configuration;

        public DoPingHandler(
            IMediator mediator,
            IRedisCache cache,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _cache = cache;
            _configuration = configuration;
        }


        public async Task Handle(DoPingMessage notification, CancellationToken cancellationToken)
        {
            var cacheKey = $"{notification.EndPointId}-{nameof(SensorType.Ping)}";
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

            var pingResult = PingUtil.Ping(notification.IpAddress);

            if (pingResult == false) // IS Network availability
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
                await _mediator.Publish(new PingNotificationMessage
                {
                    IpAddress = notification.IpAddress,
                    EmailNotify = notification.EmailNotify,
                    EndPointId = notification.EndPointId,
                    MobileNotify = notification.MobileNotify,
                    PingSuccess = pingResult
                }, cancellationToken);

            }



        }
    }
}
