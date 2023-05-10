using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Owleye.Application.Notifications.Messages;

namespace Owleye.Application.Handlers
{
    public class NotifyViaMiceosoftTeamNotificationHandler : INotificationHandler<NotifyViaMicrosoftTeamNotification>
    {
        private readonly IConfiguration _configuration;

        public NotifyViaMiceosoftTeamNotificationHandler(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task Handle(NotifyViaMicrosoftTeamNotification notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.EmailAddresses)
            {
                Console.WriteLine($"Team : {item} {notification.ServiceUrl} availably status is {notification.IsServiceAlive}");
            }
        }
    }
}
