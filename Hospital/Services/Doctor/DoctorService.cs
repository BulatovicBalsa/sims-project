﻿using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Manager;
using Hospital.Repositories.Patient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Hospital.Repositories.Doctor;

namespace Hospital.Coordinators;

public class DoctorService
{
    private readonly ExaminationRepository _examinationRepository;
    private readonly PatientRepository _patientRepository;
    private readonly RoomRepository _roomRepository;
    private readonly DoctorRepository _doctorRepository;

    public DoctorService()
    {
        _examinationRepository = new ExaminationRepository(new ExaminationChangesTracker());
        _patientRepository = new PatientRepository();
        _roomRepository = new RoomRepository();
        _doctorRepository = new DoctorRepository();
    }

    public List<Patient> GetViewedPatients(Doctor doctor)
    {
        var finishedExaminations = _examinationRepository.GetFinishedExaminations(doctor);
        var viewedPatients = finishedExaminations.Select(examination => examination.Patient).Distinct().ToList();
        return viewedPatients;
    }

    public List<Examination> GetExaminationsForNextThreeDays(Doctor doctor)
    {
        return _examinationRepository.GetExaminationsForNextThreeDays(doctor);
    }

    public Patient GetPatient(Examination examination)
    {
        return _patientRepository.GetById(examination.Patient.Id);
    }

    public List<Patient> GetAllPatients()
    {
        return _patientRepository.GetAll();
    }

    public void UpdatePatient(Patient patient)
    {
        _patientRepository.Update(patient);
    }

    public void AddExamination(Examination examination)
    {
        _examinationRepository.Add(examination, false);
    }

    public void UpdateExamination(Examination examination)
    {
        _examinationRepository.Update(examination, false);
    }

    public void DeleteExamination(Examination examination)
    {
        _examinationRepository.Delete(examination, false);
    }

    public List<Room> GetRoomsForExamination()
    {
        var allRooms = _roomRepository.GetAll();
        return allRooms.Where(room =>
            room.Type == Room.RoomType.OperatingRoom || room.Type == Room.RoomType.ExaminationRoom).ToList();
    }

    public Patient? GetPatientById(string patientId)
    {
        return _patientRepository.GetById(patientId);
    }

    public List<string> GetAllSpecializations()
    {
        var allDoctors = _doctorRepository.GetAll();

        return allDoctors.Select(doctor => doctor.Specialization).ToList();
    }
}