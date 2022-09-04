using System;
using System.Collections.Generic;
using System.Linq;

namespace Owleye.Application.Dto
{
    [Serializable]
    public class MonitoringHistoryDto
    {
        public MonitoringHistoryDto()
        {
            TimeHistories = new List<MonitoringTimeHistoryDto>();
        }
        public DateTime LastCheck { get; protected set; }
        public bool LastStatus { get; protected set; }


        public List<MonitoringTimeHistoryDto> TimeHistories { get; protected set; }

        public void AddCheckEvent(DateTime time, bool status)
        {
            if (TimeHistories == null)
            {
                TimeHistories = new List<MonitoringTimeHistoryDto>();
            }

            LastCheck = time;
            LastStatus = status;

            TimeHistories.Add(new MonitoringTimeHistoryDto().Set(time, status));
        }

        public bool HasHistory()
        {
            return TimeHistories.Any();
        }

        public DateTime GetLastTimeAvailable()
        {
            var lastAvail = TimeHistories.Where(q => q.IsAlive).OrderByDescending(q => q.CheckedTime).FirstOrDefault();
            return lastAvail != null ? lastAvail.CheckedTime : DateTime.Now;
        }


    }
}
