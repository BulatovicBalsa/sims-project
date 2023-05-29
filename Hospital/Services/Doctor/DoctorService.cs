using Hospital.Models.Doctor;
using System.Collections.Generic;
using System.Linq;
using Hospital.Repositories.Doctor;

namespace Hospital.Services;

public class DoctorService
{
    private readonly DoctorRepository _doctorRepository;

    public DoctorService()
    {
        _doctorRepository = DoctorRepository.Instance;
    }

    public List<Doctor> GetAll()
    {
        return _doctorRepository.GetAll(); 
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