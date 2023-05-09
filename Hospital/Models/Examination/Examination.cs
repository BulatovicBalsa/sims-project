using Hospital.Models.Doctor;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Examination
{
    public class UpdateExaminationDto
    {
        public DateTime Start { get; set; }
        public bool IsOperation { get; set; }
        public Room? Room { get; set; }
        public Patient.Patient Patient { get; set; }

        public UpdateExaminationDto(DateTime start, bool isOperation, Room? room, Patient.Patient patient)
        {
            Start = start;
            IsOperation = isOperation;
            Room = room;
            Patient = patient;  
        }
    }
    public class Examination
    {
        public const int DURATION = 15;
        public string Id { get; set; }
        public Doctor.Doctor? Doctor { get; set; }
        public Patient.Patient Patient { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => Start.AddMinutes(15); // NOTE: this isn't a property 
        public bool IsOperation { get; set; }
        public string Anamnesis { get; set; }
        public Room? Room { get; set; }
        public bool Admissioned { get; set; }
        public Examination(Doctor.Doctor doctor, Patient.Patient patient, bool isOperation, DateTime start, Room room)
        {
            Doctor = doctor;
            Patient = patient;
            Start = start;
            IsOperation = isOperation;
            Id = Guid.NewGuid().ToString();
            Anamnesis = "";
            Room = room;
            Admissioned = false;
        }

        public Examination()
        {
            Id = Guid.NewGuid().ToString();
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

        public bool IsPerfomable()
        {
            var now = DateTime.Now;
            return Start < now && End > now;
        }

        public Examination DeepCopy()
        {
            Examination copy = new Examination()
            {
                Id = this.Id,
                Doctor = this.Doctor.DeepCopy(), 
                Start = this.Start,
                Patient = this.Patient.DeepCopy()
            };

            return copy;
        }

        public void UpdateExamination(UpdateExaminationDto examinationDto)
        {
            Start = examinationDto.Start;
            Room = examinationDto.Room;
            IsOperation = examinationDto.IsOperation;
            Patient = examinationDto.Patient;
        }
    }
}
