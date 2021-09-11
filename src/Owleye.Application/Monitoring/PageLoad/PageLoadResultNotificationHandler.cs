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
    public class PageLoadResultNotificationHandler : INotificationHandler<PageLoadResultNotification>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;

        public PageLoadResultNotificationHandler(IMediator mediator, IRedisCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task Handle(PageLoadResultNotification notification, CancellationToken cancellationToken)
        {
            MonitoringHistoryDto history = null;

            //TODO  extension
            var cacheKey =
                $"{notification.EndPointId}-{nameof(SensorType.PageLoad)}-{DateTime.Now.ToString(@"yyyy-MM-dd")}";

            history = await _cache.GetAsync<MonitoringHistoryDto>(cacheKey) ?? new MonitoringHistoryDto();

            notification.LastAvilable = history.GetLastAvailable();

            if (history.HasHistory() && history.LastStatus != notification.LoadSuccess)
            {
                await Notify(notification, cancellationToken);
            }

            history.AddCheckEvent(DateTime.Now, notification.LoadSuccess);
            await _cache.SetAsync(cacheKey, history);
        }

        private async Task Notify(PageLoadResultNotification notification, CancellationToken cancellationToken)
        {
            if (notification.EmailNotify.IsNotNullOrEmpty())
            {
                await _mediator.Publish(new NotifyViaEmailNotification
                {
                    ServiceUrl = notification.PageUrl,
                    SensorType = SensorType.PageLoad,
                    EmailAddresses = notification.EmailNotify,
                    IsServiceAlive = notification.LoadSuccess,
                    LastAvailable = notification.LastAvilable
                }, cancellationToken);
            }

            if (notification.MobileNotify.IsNotNullOrEmpty())
            {
                //todo notify via sms.
            }
        }
    }
}
