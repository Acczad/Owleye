using System.Linq;
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
            var endPointList = notification.EndPointList;

            foreach (var sensor in endPointList)
            {
                var phoneList = sensor.EndPoint.Notification.Select(q => q.PhoneNumber).ToList();
                var emailList = sensor.EndPoint.Notification.Select(q => q.EmailAddress).ToList();

                switch (sensor.SensorType)
                {
                    case SensorType.Ping:
                        {
                            await _mediator.Publish(
                                new DoPingNotification
                                {
                                    IpAddress = sensor.EndPoint.IpAddress,
                                    MobileNotify = phoneList,
                                    EndPointId = sensor.EndPointId,
                                    EmailNotify = emailList
                                }
                            );

                            break;
                        }

                    case SensorType.PageLoad:
                        {
                            await _mediator.Publish(
                                new DoPageLoadNotification
                                {
                                    PageUrl = sensor.EndPoint.Url,
                                    MobileNotify = phoneList,
                                    EndPointId = sensor.EndPointId,
                                    EmailNotify = emailList,
                                }
                            );

                            break;
                        }
                }

            }
        }


    }
}
