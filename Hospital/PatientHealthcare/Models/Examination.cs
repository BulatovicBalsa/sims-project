﻿using System;
using System.Collections.Generic;
using System.Text;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
using Hospital.Workers.Models;

namespace Hospital.PatientHealthcare.Models;

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

    public Examination(Doctor? doctor, Patient patient, bool isOperation, DateTime start, Room? room,
        bool urgent = false, List<Doctor>? procedureDoctors = null, List<Nurse>? procedureNursesIds = null)
    {
        Doctor = doctor;
        Patient = patient;
        Start = start;
        IsOperation = isOperation;
        Id = Guid.NewGuid().ToString();
        Anamnesis = "";
        Room = room ?? RoomRepository.Instance.GetAll()[0];
        Admissioned = false;
        Urgent = urgent;
        ProcedureDoctors = procedureDoctors;
        ProcedureNurses = procedureNursesIds;
    }

    public Examination()
    {
        Id = Guid.NewGuid().ToString();
        Room = RoomRepository.Instance.GetAll()[0];
    }

    public string Id { get; set; }
    public Doctor? Doctor { get; set; }
    public Patient Patient { get; set; }
    public DateTime Start { get; set; }
    public DateTime End => Start.AddMinutes(15); // NOTE: this isn't a property 
    public bool IsOperation { get; set; }
    public string Anamnesis { get; set; }
    public Room? Room { get; set; }
    public bool Admissioned { get; set; }
    public bool Urgent { get; set; }
    public List<Doctor>? ProcedureDoctors { get; set; }
    public List<Nurse>? ProcedureNurses { get; set; }

    public string ToStringCli
    {
        get
        {
            var indent = "\t\t";
            var builder = new StringBuilder();
            builder.Append(
                $"Examination data: Patient: {Patient.FirstName} {Patient.LastName} {Patient.Jmbg}, Start: {Start}, Operation?: {IsOperation}");
            if (ProcedureDoctors?.Count > 0)
            {
                builder.Append($"\n{indent}Procedure Doctors: ");
                ProcedureDoctors.ForEach(doctor => builder.Append($"\n\t{indent}{doctor}"));
            }

            if (!(ProcedureNurses?.Count > 0)) return builder.ToString();
            builder.Append($"\n{indent}Procedure Nurses: ");
            ProcedureNurses.ForEach(nurse => builder.Append($"\n\t{indent}{nurse}"));

            return builder.ToString();
        }
    }

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
        var difference = Start - DateTime.Now;
        return Math.Abs(difference.TotalMinutes) <= 15;
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

    public override string ToString()
    {
        return
            $"Doctor: {Doctor?.FirstName ?? ""} {Doctor?.LastName ?? ""}, Patient: {Patient.FirstName} {Patient.LastName}, Start: {Start}";
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