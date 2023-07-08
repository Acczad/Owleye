using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnumsNET;
using MediatR;
using Microsoft.Extensions.Configuration;
using Owleye.Application.Notifications.Messages;
using Owleye.Domain;
using Owleye.Shared.MicrosoftTeams;
using Owleye.Shared.Model.MicrosoftTeams;
using Owleye.Shared.Extentions;
using Owleye.Shared.Cache;
using System.Runtime.CompilerServices;
using Owleye.Application.Monitoring.NotifyToUser;

namespace Owleye.Application.Handlers
{
    public class NotifyViaMicrosoftTeamNotificationHandler : INotificationHandler<NotifyViaMicrosoftTeamNotification>
    {
        private readonly IMicrosoftTeamsService _microsoftTeamsService;
        private readonly IMessageService _messageService;

        public NotifyViaMicrosoftTeamNotificationHandler(
            IMicrosoftTeamsService microsoftTeamsService,
            IMessageService messageService)
        {
            _microsoftTeamsService=microsoftTeamsService;
            _messageService=messageService;
        }
        public async Task Handle(NotifyViaMicrosoftTeamNotification notification, CancellationToken cancellationToken)
        {

            var message = notification.IsServiceAlive ? "OK." : "Failed!";
            var color = notification.IsServiceAlive ? "green" : "red";

            await _microsoftTeamsService
                   .SendTeamsMessageAsync(new SendTeamsUsersMessageRequest
                   {
                       Emails = notification.EmailAddresses,
                       Message= $"{notification.SensorType.AsString(EnumFormat.Description)} for " +
                       $"<a href=\"{notification.ServiceUrl}\" bgcolor=\"{color}\">{notification.Name}</a> {message}"
                   });
        }
    }
}

   


