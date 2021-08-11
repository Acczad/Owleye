﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiteX.Email.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Owleye.Core.Notifications.Messages;
using Owleye.Core.Services;

namespace Owleye.Core.Handlers
{
    public class NotifyViaEmailHandler : INotificationHandler<NotifyViaEmailMessage>
    {
        private readonly ILiteXEmailSender _emailSender;
        private readonly ILogger<NotifyViaEmailHandler> _logger;
        private readonly IConfiguration _configuration;

        public NotifyViaEmailHandler(ILiteXEmailSender emailSender,
            ILogger<NotifyViaEmailHandler> logger,
            IConfiguration configuration)
        {
            _emailSender = emailSender;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task Handle(NotifyViaEmailMessage notification, CancellationToken cancellationToken)
        {
            var message = NotifyMessagePreparationService.Prepare(notification);

            _logger.LogInformation($"Endpoint {notification.ServiceUrl} availabily status is {notification.IsServiceAlive}");

            var mainEmailAddress = notification.EmailAddresses.First(); // TODO fix this, this is random pick email address.
            var bccAddresses = notification.EmailAddresses.Skip(1)?.ToList();
            if (bccAddresses.Any() == false)
                bccAddresses = null;

            var mailTitle = NotifyMessagePreparationService.PrepareMailTitle(notification.ServiceUrl, notification.IsServiceAlive);
            var from = _configuration["MailNotify:FromMail"];
            var fromName = _configuration["MailNotify:FromName"];
            var toName = _configuration["MailNotify:ToName"];
            
            await _emailSender.SendEmailAsync(mailTitle, message,
            from, fromName,
            mainEmailAddress,
            toName, bcc: bccAddresses, cancellationToken: cancellationToken);

        }
    }
}
