using System;
using Hospital.Models.Doctor;
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
        _examinationRepository = new ExaminationRepository();
        _patientRepository = new PatientRepository();
        _doctorRepository = DoctorRepository.Instance;
        _roomRepository = RoomRepository.Instance;
    }

    public Patient GetPatient(Examination examination)
    {
        return _patientRepository.GetById(examination.Patient!.Id)!;
    }

    public List<Patient> GetAllPatients()
    {
        return _patientRepository.GetAll();
    }

    public void UpdatePatient(Patient patient)
    {
        _patientRepository.Update(patient);
    }

    public List<Room> GetRoomsForExamination()
    {
        var allRooms = _roomRepository.GetAll();
        return allRooms.Where(room =>
            room.Type == RoomType.OperatingRoom || room.Type == RoomType.ExaminationRoom).ToList();
    }

    public Patient? GetPatientById(string patientId)
    {
        return _patientRepository.GetById(patientId);
    }

    public List<string> GetAllSpecializations()
    {
        var allDoctors = _doctorRepository.GetAll();

        return allDoctors.Select(doctor => doctor.Specialization).Distinct().ToList();
    }

    public List<Doctor> GetQualifiedDoctors(string specialization)
    {
        var allDoctors = _doctorRepository.GetAll();

        return allDoctors.Where(doctor => doctor.Specialization == specialization).ToList();
    }
}