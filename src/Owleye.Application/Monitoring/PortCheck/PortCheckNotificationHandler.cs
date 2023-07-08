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
    public class PortCheckNotificationHandler : INotificationHandler<PortCheckResultNotification>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly INotifyDispatcherService _notifyDispatcherService;

        public PortCheckNotificationHandler(
            IMediator mediator, IRedisCache cache, INotifyDispatcherService notifyDispatcherService)
        {
            _mediator = mediator;
            _cache = cache;
            _notifyDispatcherService = notifyDispatcherService;
        }
        public async Task Handle(PortCheckResultNotification notification, CancellationToken cancellationToken)
        {
            var cacheKey = $"{notification.EndPointId}-{nameof(SensorType.PortCheck)}-{DateTime.Now.ToString(@"yyyy-MM-dd")}";

            var history = await _cache.GetAsync<MonitoringHistoryDto>(cacheKey) ?? new MonitoringHistoryDto();

            if (history.LastStatus != notification.PortCheckSuccess)
            {
                await _notifyDispatcherService.Notify(
                    new NotifyInfo
                    {
                        endPointAddress = notification.IpAddress,
                        notificationList = notification.NotificationList,
                        sensorType = SensorType.PortCheck,
                        sensorAvailability = notification.PortCheckSuccess,
                        Name = notification.Name,
                    });
            }

            history.AddCheckEvent(DateTime.Now, notification.PortCheckSuccess);
            await _cache.SetAsync(cacheKey, history);
        }
    }
}
