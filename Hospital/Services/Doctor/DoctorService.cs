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
    private readonly RoomRepository _roomRepository;
    private readonly DoctorRepository _doctorRepository;

    public DoctorService()
    {
        _doctorRepository = DoctorRepository.Instance;
        _roomRepository = RoomRepository.Instance;
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