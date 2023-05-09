using Hospital.Models.Manager;
using System;

namespace Hospital.Models.Examination;
using Doctor;
using Patient;

public class UpdateExaminationDto
{
    public UpdateExaminationDto(DateTime start, bool isOperation, Room? room, Patient patient,
        Doctor? doctor)
    {
        Start = start;
        IsOperation = isOperation;
        Room = room;
        Patient = patient;
        Doctor = doctor;
    }

    public DateTime Start { get; set; }
    public bool IsOperation { get; set; }
    public Room? Room { get; set; }
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
}

public class Examination
{
    public const int Duration = 15;

    public Examination(Doctor doctor, Patient patient, bool isOperation, DateTime start, Room room)
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
        Anamnesis = "";
    }

    public string Id { get; set; }
    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
    public DateTime Start { get; set; }
    public DateTime End => Start.AddMinutes(15); // NOTE: this isn't a property 
    public bool IsOperation { get; set; }
    public string Anamnesis { get; set; }
    public Room? Room { get; set; }
    public bool Admissioned { get; set; }

    public bool DoesInterfereWith(Examination otherExamination)
    {
        return DoesInterfereWith(otherExamination.Start);
    }

    public bool DoesInterfereWith(DateTime start)
    {
        var end = start.AddMinutes(Duration);
        var startOverlap = End > start;
        var endOverlap = Start < end;

        return startOverlap && endOverlap;
    }

    public bool IsPerformable()
    {
        var now = DateTime.Now;
        return Start < now && End > now;
    }

    public Examination DeepCopy()
    {
        var copy = new Examination
        {
            Id = Id,
            Doctor = Doctor?.DeepCopy(),
            Start = Start,
            Patient = Patient?.DeepCopy()
        };

        return copy;
    }

    public void Update(UpdateExaminationDto examinationDto)
    {
        Start = examinationDto.Start;
        Room = examinationDto.Room;
        IsOperation = examinationDto.IsOperation;
        Patient = examinationDto.Patient;
        Doctor = examinationDto.Doctor;
    }
}