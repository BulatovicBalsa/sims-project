using Hospital.Models.Doctor;
using System.Collections.Generic;
using System.Linq;
using Hospital.Repositories.Doctor;
using Hospital.DTOs;

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

    public Doctor? GetById(string id)
    {
        return _doctorRepository.GetById(id);
    }

    public List<string> GetAllSpecializations()
    {
        return _doctorRepository.GetAllSpecializations();
    }

    public List<Doctor> GetQualifiedDoctors(string specialization)
    {
        return _doctorRepository.GetQualifiedDoctors(specialization);
    }
    public List<PersonDTO> GetDoctorsAsPersonDTOsByFilter(string id,string searchText)
    {
        return _doctorRepository.GetDoctorsAsPersonDTOsByFilter(id, searchText);
    }
}