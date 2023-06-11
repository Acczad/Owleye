using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Owleye.Application.Notifications.Messages;
using Owleye.Shared.MicrosoftTeams;
using Owleye.Shared.Model.MicrosoftTeams;

namespace Owleye.Application.Handlers
{
    public class NotifyViaMicrosoftTeamNotificationHandler : INotificationHandler<NotifyViaMicrosoftTeamNotification>
    {
        private readonly IConfiguration _configuration;
        private readonly IMicrosoftTeamsService _microsoftTeamsService;

        public NotifyViaMicrosoftTeamNotificationHandler(
            IConfiguration configuration,
            IMicrosoftTeamsService microsoftTeamsService)
        {
            _configuration = configuration;
            _microsoftTeamsService=microsoftTeamsService;
        }
        public async Task Handle(NotifyViaMicrosoftTeamNotification notification, CancellationToken cancellationToken)
        {
            await _microsoftTeamsService
                    .SendTeamsMessageAsync(new SendTeamsUsersMessageRequest
                    {
                        Emails = notification.EmailAddresses,
                        Message= $"{notification.ServiceUrl} availably status is {notification.IsServiceAlive}"
                    });
            
        }
    }
}
