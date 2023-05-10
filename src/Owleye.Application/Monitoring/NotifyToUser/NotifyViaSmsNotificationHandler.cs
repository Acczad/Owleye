using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Owleye.Application.Notifications.Messages;

namespace Owleye.Application.Handlers
{
    public class NotifyViaSmsNotificationHandler : INotificationHandler<NotifyViaSmsNotification>
    {
        private readonly IConfiguration _configuration;

        public NotifyViaSmsNotificationHandler(
            ILogger<NotifyViaSmsNotificationHandler> logger,
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task Handle(NotifyViaSmsNotification notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.PhoneNumbers)
            {
                Console.WriteLine($"SMS: {item} {notification.ServiceUrl} availably status is {notification.IsServiceAlive}");
            }
        }
    }
}
