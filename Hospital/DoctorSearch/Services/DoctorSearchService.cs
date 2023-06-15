using System.Collections.Generic;
using Hospital.Injectors;
using Hospital.Serialization;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace Hospital.DoctorSearch.Services;

internal class DoctorSearchService
{
    private readonly DoctorRepository _doctorRepository;
    private List<Doctor> _filteredDoctors;

    public DoctorSearchService()
    {
        _doctorRepository =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
        _filteredDoctors = new List<Doctor>();
    }

    public List<Doctor> GetAllDoctors()
    {
        return _doctorRepository.GetAll();
    }

    public void FilterDoctors(string firstName, string lastName, string specialization)
    {
        _filteredDoctors = _doctorRepository.GetDoctorsByFilter(firstName, lastName, specialization);
    }

    public List<Doctor> GetFilteredDoctors()
    {
        return _filteredDoctors;
    }
}