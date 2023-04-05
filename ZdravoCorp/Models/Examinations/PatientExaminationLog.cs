using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Patients;

namespace ZdravoCorp.Models.Examinations
{
    internal class PatientExaminationLog
    {
        public Patient Patient { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsCreationLog { get; set; } 

        public PatientExaminationLog(Patient patient, DateTime timestamp,bool isCreationLog)
        {
            Patient = patient;
            Timestamp = timestamp;
            IsCreationLog = isCreationLog;
        }


    }
}
