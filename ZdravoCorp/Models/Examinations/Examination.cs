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
        public const int DURATION = 15;
        static int id = 0;
        public int Id { get; set; }
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
            Id = id;
            id++;
        }

        public bool DoesInterfereWith(Examination otherExamination)
        {
            return DoesInterfereWith(otherExamination.Start);
        }

        public bool DoesInterfereWith(DateTime start)
        {
            DateTime end = start.AddMinutes(DURATION);
            bool startOverlap = this.End > start;
            bool endOverlap = this.Start < end;

            return startOverlap && endOverlap;
        }
    }
}
