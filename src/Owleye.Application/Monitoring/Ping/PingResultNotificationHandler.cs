using System;
using System.Threading;
using System.Threading.Tasks;
using Extension.Methods;
using MediatR;
using Owleye.Shared.Cache;
using Owleye.Application.Dto;
using Owleye.Application.Notifications.Messages;
using Owleye.Domain;

namespace Owleye.Application.Handlers
{
    public class PingResultNotificationHandler : INotificationHandler<PingResultNotification>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;

        public PingResultNotificationHandler(IMediator mediator, IRedisCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }
        public async Task Handle(PingResultNotification notification, CancellationToken cancellationToken)
        {
            MonitoringHistoryDto history = null;

            //TODO  extension
            var cacheKey =
                $"{notification.EndPointId}-{nameof(SensorType.Ping)}-{DateTime.Now.ToString(@"yyyy-MM-dd")}";

            history = await _cache.GetAsync<MonitoringHistoryDto>(cacheKey) ?? new MonitoringHistoryDto();

            if (history.LastStatus != notification.PingSuccess)
            {
                await Notify(notification, cancellationToken);
            }

            history.AddCheckEvent(DateTime.Now, notification.PingSuccess);
            await _cache.SetAsync(cacheKey, history);

        }

        private async Task Notify(PingResultNotification notification, CancellationToken cancellationToken)
        {
            if (notification.EmailNotify.IsNotNullOrEmpty())
            {
                await _mediator.Publish(new NotifyViaEmailNotification
                {
                    IpAddress = notification.IpAddress,
                    SensorType = SensorType.Ping,
                    EmailAddresses = notification.EmailNotify,
                    IsServiceAlive = notification.PingSuccess
                }, cancellationToken);
            }

            if (notification.MobileNotify.IsNotNullOrEmpty())
            {
                //todo notify via sms.
            }
        }
    }
}
