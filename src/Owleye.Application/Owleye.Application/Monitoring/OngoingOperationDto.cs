using System;

namespace Owleye.Application.Dto
{
    [Serializable]
    public class OngoingOperationDto
    {
        public OngoingOperationDto(DateTime startDate)
        {
            StartDate = startDate;
        }
        public DateTime StartDate { get; protected set; }
    }
}
