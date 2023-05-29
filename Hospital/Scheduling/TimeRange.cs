using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Scheduling
{
    public class TimeRange
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool OverlapsWith(TimeRange other)
        {
            return other.StartTime < EndTime && other.EndTime > StartTime;
        }

        public bool OverlapsWith(DateTime start, DateTime end)
        {
            return OverlapsWith(new TimeRange(start, end));
        }
    }
}
