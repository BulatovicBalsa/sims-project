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
        public DateTime Timestamp { get; }
        public bool IsCreationLog { get; set; }

        public PatientExaminationLog(Patient patient, bool isCreationLog)
        {
            Patient = patient;
            Timestamp = DateTime.Now;
            IsCreationLog = isCreationLog;
        }


    }
}
