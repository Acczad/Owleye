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
    public class NotifyViaDiscordNotificationHandler : INotificationHandler<NotifyViaDiscordNotification>
    {
        private readonly ILogger<NotifyViaDiscordNotification> _logger;
        private readonly IConfiguration Configuration;

        public NotifyViaDiscordNotificationHandler(
            ILogger<NotifyViaDiscordNotification> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }
        public async Task Handle(NotifyViaDiscordNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Endpoint {notification.ServiceUrl} availabily status is {notification.IsServiceAlive}");

            foreach (var hookAddress in notification.DiscordHookApis)
            {
                NotifyViaDiscord(
                notification.ServiceUrl,
                notification.IsServiceAlive,
                "~",
                hookAddress);
            }

        }

        public void NotifyViaDiscord(string endpointAddress,
         bool availability,
         string endpointName,
         string hookAddress)
        {
            DiscordWebhook hook = new DiscordWebhook();

            var name = "Monitor";
            hook.Url = hookAddress;
            DiscordMessage discordMessage = new DiscordMessage();
            discordMessage.TTS = false;
            discordMessage.Username = name;
            discordMessage.AvatarUrl = Configuration["DiscordNotify:AvatarUrl"];
            discordMessage.Embeds = new System.Collections.Generic.List<DiscordEmbed>();
            var embed = new DiscordEmbed();
            embed.Footer = new EmbedFooter
            {
                Text = $"DateTime : {DateTime.Now}"
            };
            embed.Author = new EmbedAuthor
            {
                Name = $"Owleye {endpointName } {availability.ToString()}",
                IconUrl =  Configuration["DiscordNotify:IconUrl"]
            };
            embed.Color = availability == true ? Color.YellowGreen : Color.Red;
            embed.Fields = new System.Collections.Generic.List<EmbedField>
            {
                new EmbedField{Name="Name:",InLine=true,Value=endpointName},
                new EmbedField{Name="Address:",InLine=true,Value=endpointAddress},
                new EmbedField{Name="Availability:",InLine=true,Value=availability.ToString()},
            };
            discordMessage.Embeds.Add(embed);
            hook.Send(discordMessage);
        }


    }
}
