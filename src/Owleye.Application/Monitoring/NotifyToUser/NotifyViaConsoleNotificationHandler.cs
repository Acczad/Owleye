using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using CSharpDiscordWebhook.NET.Discord;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Owleye.Application.Notifications.Messages;

namespace Owleye.Application.Handlers
{
    public class NotifyViaConsoleNotificationHandler : INotificationHandler<NotifyViaConsoleNotification>
    {
        private readonly ILogger<NotifyViaConsoleNotification> _logger;
        private readonly IConfiguration Configuration;

        public NotifyViaConsoleNotificationHandler(
            ILogger<NotifyViaConsoleNotification> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }
        public async Task Handle(NotifyViaConsoleNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Console: {notification.ServiceUrl} availably status is {notification.IsServiceAlive}");
        }
    }
}
