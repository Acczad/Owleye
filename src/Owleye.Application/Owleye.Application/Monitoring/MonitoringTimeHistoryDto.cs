using System;

namespace Owleye.Application.Dto
{
    [Serializable]
    public class MonitoringTimeHistoryDto
    {
        public DateTime CheckedTime { get; protected set; }
        public bool IsAlive { get; protected set; }

        public MonitoringTimeHistoryDto Set(DateTime checkedTime, bool isAlive)
        {
            CheckedTime = checkedTime;
            IsAlive = isAlive;
            return this;
        }
    }
}
