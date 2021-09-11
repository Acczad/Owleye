using MediatR;
using Owleye.Domain;
using System.Collections.Generic;

namespace Owleye.Application.Dto.Messages
{
    public class EndPointCheckMessage : INotification
    {
        public IEnumerable<SensorDto> EndPointList { get; set; }
    }
}
