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
    public class PageLoadResultNotificationHandler : INotificationHandler<PageLoadResultNotification>
    {
        private readonly IRedisCache _cache;
        private readonly INotifyDispatcherService _notifyDispatcherService;

        public PageLoadResultNotificationHandler(
           IRedisCache cache, INotifyDispatcherService notifyDispatcherService)
        {
            _cache = cache;
            _notifyDispatcherService = notifyDispatcherService;
        }

        public async Task Handle(PageLoadResultNotification notification, CancellationToken cancellationToken)
        {
            MonitoringHistoryDto monitoringHistory = null;

            //TODO  extension
            var cacheKey =
                $"{notification.EndPointId}-{nameof(SensorType.PageLoad)}-{DateTime.Now.ToString(@"yyyy-MM-dd")}";

            monitoringHistory = await _cache.GetAsync<MonitoringHistoryDto>(cacheKey) ?? new MonitoringHistoryDto();

            notification.LastAvilable = monitoringHistory.GetLastTimeAvailable();

            // first check and down status
            if (monitoringHistory.HasHistory() == false && notification.LoadSuccess == false)
            {
                await _notifyDispatcherService.Notify(
                    new NotifyInfo
                    {
                        endPointAddress = notification.PageUrl,
                        notificationList = notification.NotificationList,
                        sensorType = SensorType.PageLoad,
                        sensorAvailability = notification.LoadSuccess
                    });
            }

            // change status in endpoint status
            if (monitoringHistory.HasHistory() && monitoringHistory.LastStatus != notification.LoadSuccess)
            {
                await _notifyDispatcherService.Notify(new NotifyInfo
                {
                    endPointAddress = notification.PageUrl,
                    notificationList = notification.NotificationList,
                    sensorType = SensorType.PageLoad,
                    sensorAvailability = notification.LoadSuccess
                });
            }

            monitoringHistory.AddCheckEvent(DateTime.Now, notification.LoadSuccess);
            await _cache.SetAsync(cacheKey, monitoringHistory);
        }
    }
}
