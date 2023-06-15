using System.Collections.Generic;
using Hospital.Core.Accounts.DTOs;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.Workers.Services;

public class DoctorService
{
    private readonly DoctorRepository _doctorRepository;

    public DoctorService()
    {
        _doctorRepository =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
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

    public List<PersonDTO> GetDoctorsAsPersonDTOsByFilter(string id, string searchText)
    {
        return _doctorRepository.GetDoctorsAsPersonDTOsByFilter(id, searchText);
    }
}