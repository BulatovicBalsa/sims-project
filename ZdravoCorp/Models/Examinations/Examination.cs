using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Models.Patients;

namespace ZdravoCorp.Models
{
    public class Examination
    {
        const int DURATION = 15;
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get { return Start.AddMinutes(15); } } // NOTE: this isn't a property 
        public bool IsOperation { get; set; }

        public Examination(Doctor doctor, Patient patient, bool isOperation, DateTime start)
        {
            Doctor = doctor;
            Patient = patient;
            Start = start;
            IsOperation = isOperation;
        }
    }
}
