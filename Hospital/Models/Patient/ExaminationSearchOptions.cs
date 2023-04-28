using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    using Hospital.Models.Doctor;
    public enum Priority
    {
        Doctor,
        TimeRange
    }
    public class ExaminationSearchOptions
    {
        public Priority Priority { get; set; }
        public Doctor PreferredDoctor { get; set; }
        public DateTime LatestDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ExaminationSearchOptions(Doctor preferredDoctor, DateTime latestDate, TimeSpan startTime, TimeSpan endTime,Priority priority)
        {
            Priority = priority;
            PreferredDoctor = preferredDoctor;
            LatestDate = latestDate;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
