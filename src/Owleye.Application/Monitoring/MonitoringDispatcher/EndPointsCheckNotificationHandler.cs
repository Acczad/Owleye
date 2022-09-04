﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Owleye.Domain;
using Owleye.Application.Dto.Messages;

namespace Owleye.Application.Handlers
{
    public class EndPointsCheckNotificationHandler : INotificationHandler<EndPointsCheckNotification>
    {
        private readonly IMediator _mediator;

        public EndPointsCheckNotificationHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(EndPointsCheckNotification notification, CancellationToken cancellationToken)
        {
            var sensorList = notification.Sensors;

            foreach (var sensor in sensorList)
            {
                var notifList = sensor.EndPoint.Notification
                    .GroupBy(g=>g.NotificationType)
                    .ToDictionary(q => q.Key ,q=>q.ToList().Select(q=>q.NoTificationAddress).ToList());

                switch (sensor.SensorType)
                {
                    case SensorType.Ping:
                        {
                            await _mediator.Publish(
                                new DoPingNotification
                                {
                                    IpAddress = sensor.EndPoint.IpAddress,
                                    EndPointId = sensor.EndPointId,
                                    NotificationList= notifList
                                });
                            break;
                        }
                    case SensorType.PageLoad:
                        {
                            await _mediator.Publish(
                                new DoPageLoadNotification
                                {
                                    PageUrl = sensor.EndPoint.Url,
                                    NotificationList= notifList,
                                    EndPointId = sensor.EndPointId,
                                });

                            break;
                        }
                }
            }
        }
    }
}
