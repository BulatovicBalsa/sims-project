using System;
using Newtonsoft.Json;

namespace Hospital.Core.Scheduling;

public class TimeRange
{
    public TimeRange(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    [JsonProperty("StarTime")] public DateTime StartTime { get; set; }

    [JsonProperty("EndTime")] public DateTime EndTime { get; set; }

    public bool DoesOverlapWith(TimeRange other)
    {
        return other.StartTime < EndTime && other.EndTime > StartTime;
    }

    public bool DoesOverlapWith(DateTime start, DateTime end)
    {
        return DoesOverlapWith(new TimeRange(start, end));
    }
}