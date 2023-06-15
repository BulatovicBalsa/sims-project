using System.Collections.Generic;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.DoctorSearch.Services;

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