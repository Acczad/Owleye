using MediatR;
using System.Collections.Generic;

namespace Owleye.Application.Dto.Messages
{
    public class EndPointsCheckNotification : INotification
    {
        public IEnumerable<SensorDto> EndPointList { get; set; }
    }
}
