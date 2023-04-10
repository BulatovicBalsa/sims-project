using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class PatientExaminationLog
    {
        public Patient Patient { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsCreationLog { get; set; }

        public PatientExaminationLog(Patient patient, bool isCreationLog)
        {
            Patient = patient;
            Timestamp = DateTime.Now;
            IsCreationLog = isCreationLog;
        }

        public PatientExaminationLog() {}

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PatientExaminationLog other = (PatientExaminationLog)obj;

            // Compare Timestamp values with second precision
            bool timestampEqual = Timestamp.Year == other.Timestamp.Year
                                  && Timestamp.Month == other.Timestamp.Month
                                  && Timestamp.Day == other.Timestamp.Day
                                  && Timestamp.Hour == other.Timestamp.Hour
                                  && Timestamp.Minute == other.Timestamp.Minute
                                  && Timestamp.Second == other.Timestamp.Second;

            return Patient.Equals(other.Patient)
                   && timestampEqual
                   && IsCreationLog == other.IsCreationLog;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Patient.GetHashCode();
            hash = hash * 23 + Timestamp.GetHashCode();
            hash = hash * 23 + IsCreationLog.GetHashCode();
            return hash;
        }

    }
}
