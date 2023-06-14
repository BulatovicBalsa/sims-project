using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Services
{
    class DoctorSearchService
    {
        private DoctorRepository _doctorRepository;
        private List<Doctor> _filteredDoctors;

        public DoctorSearchService()
        {
            _doctorRepository = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
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
}
