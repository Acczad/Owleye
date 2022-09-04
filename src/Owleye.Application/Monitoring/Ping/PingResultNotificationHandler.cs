using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Owleye.Shared.Cache;
using Owleye.Application.Dto;
using Owleye.Application.Notifications.Messages;
using Owleye.Domain;
using Owleye.Application.Monitoring.NotifyToUser;

namespace Owleye.Application.Handlers
{
    public class PingResultNotificationHandler : INotificationHandler<PingResultNotification>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly INotifyDispatcherService _notifyDispatcherService;

        public PingResultNotificationHandler(
            IMediator mediator, IRedisCache cache, INotifyDispatcherService notifyDispatcherService)
        {
            _mediator = mediator;
            _cache = cache;
            _notifyDispatcherService = notifyDispatcherService;
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
                await _notifyDispatcherService.Notify(
                    new NotifyInfo
                    {
                        endPointAddress = notification.IpAddress,
                        notificationList = notification.NotificationList,
                        sensorType = SensorType.Ping,
                        sensorAvailability = notification.PingSuccess
                    });
            }

            history.AddCheckEvent(DateTime.Now, notification.PingSuccess);
            await _cache.SetAsync(cacheKey, history);
        }
    }
}
