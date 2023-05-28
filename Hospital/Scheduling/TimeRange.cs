using System;

namespace Hospital.Scheduling;

public class TimeRange
{
    public TimeRange(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public bool DoesOverlapWith(TimeRange other)
    {
        return other.StartTime < EndTime && other.EndTime > StartTime;
    }

    public bool DoesOverlapWith(DateTime start, DateTime end)
    {
        return DoesOverlapWith(new TimeRange(start, end));
    }
}