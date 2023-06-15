using System;
using Hospital.Core.Workers.Models;

namespace Hospital.Core.Scheduling.Models;

public enum Priority
{
    Doctor,
    TimeRange
}

public class ExaminationSearchOptions
{
    public ExaminationSearchOptions(Doctor preferredDoctor, DateTime latestDate, TimeSpan startTime, TimeSpan endTime,
        Priority priority)
    {
        Priority = priority;
        PreferredDoctor = preferredDoctor;
        LatestDate = latestDate;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Priority Priority { get; set; }
    public Doctor PreferredDoctor { get; set; }
    public DateTime LatestDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}