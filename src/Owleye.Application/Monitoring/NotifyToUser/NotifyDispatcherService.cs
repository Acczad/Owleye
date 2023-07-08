using MediatR;
using Owleye.Application.Notifications.Messages;
using Owleye.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Owleye.Application.Monitoring.NotifyToUser
{
    public interface INotifyDispatcherService
    {
        Task Notify(NotifyInfo notifyInfo);
    }
    public class NotifyInfo
    {
        public string endPointAddress { get; set; }
        public Dictionary<NotificationType, List<string>> notificationList { get; set; }
        public SensorType sensorType { get; set; }
        public bool sensorAvailability { get; set; }
        public DateTime? lastAvailable { get; set; }
        public string Name { get; set; }
    }

    public class NotifyDispatcherService : INotifyDispatcherService
    {
        private readonly IMediator _mediator;
        public NotifyDispatcherService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Notify(NotifyInfo notifyInfo)
        {
            if (notifyInfo.notificationList.ContainsKey(NotificationType.Email))
            {
                await _mediator.Publish(new NotifyViaEmailNotification
                {
                    ServiceUrl = notifyInfo.endPointAddress,
                    SensorType = notifyInfo.sensorType,
                    EmailAddresses = notifyInfo.notificationList[NotificationType.Email],
                    IsServiceAlive = notifyInfo.sensorAvailability,
                    LastAvailable = notifyInfo.lastAvailable,
                    Name=notifyInfo.Name
                });
            }

            if (notifyInfo.notificationList.ContainsKey(NotificationType.Console))
            {
                await _mediator.Publish(new NotifyViaConsoleNotification
                {
                    ServiceUrl = notifyInfo.endPointAddress,
                    SensorType = notifyInfo.sensorType,
                    IsServiceAlive = notifyInfo.sensorAvailability,
                    LastAvailable = notifyInfo.lastAvailable,
                    Name=notifyInfo.Name
                });
            }

            if (notifyInfo.notificationList.ContainsKey(NotificationType.Sms))
            {
                await _mediator.Publish(new NotifyViaSmsNotification
                {
                    ServiceUrl = notifyInfo.endPointAddress,
                    SensorType = notifyInfo.sensorType,
                    PhoneNumbers = notifyInfo.notificationList[NotificationType.Sms],
                    IsServiceAlive = notifyInfo.sensorAvailability,
                    LastAvailable = notifyInfo.lastAvailable,
                    Name=notifyInfo.Name
                });
            }

            if (notifyInfo.notificationList.ContainsKey(NotificationType.Discord))
            {
                await _mediator.Publish(new NotifyViaDiscordNotification
                {
                    ServiceUrl = notifyInfo.endPointAddress,
                    SensorType = notifyInfo.sensorType,
                    DiscordHookApis = notifyInfo.notificationList[NotificationType.Discord],
                    IsServiceAlive = notifyInfo.sensorAvailability,
                    LastAvailable = notifyInfo.lastAvailable,
                    Name=notifyInfo.Name
                });
            }

            if (notifyInfo.notificationList.ContainsKey(NotificationType.MicrosoftTeam))
            {
                await _mediator.Publish(new NotifyViaMicrosoftTeamNotification
                {
                    ServiceUrl = notifyInfo.endPointAddress,
                    SensorType = notifyInfo.sensorType,
                    EmailAddresses = notifyInfo.notificationList[NotificationType.MicrosoftTeam],
                    IsServiceAlive = notifyInfo.sensorAvailability,
                    LastAvailable = notifyInfo.lastAvailable,
                    Name=notifyInfo.Name
                });
            }
        }
    }
}
