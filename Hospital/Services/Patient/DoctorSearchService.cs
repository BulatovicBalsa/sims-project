using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    class DoctorSearchService
    {
        private DoctorRepository _doctorRepository;
        private List<Doctor> _filteredDoctors;

        public DoctorSearchService()
        {
            _doctorRepository = DoctorRepository.Instance;
            _filteredDoctors = new List<Doctor>();
        }
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAll();
        }
        public void FilterDoctors(string firstName, string lastName, string specialization)
        {
            _filteredDoctors = _doctorRepository.GetAll()
                .Where(doctor => 
                    doctor.FirstName.ToLower().Contains(firstName.ToLower()) &&
                    doctor.LastName.ToLower().Contains(lastName.ToLower()) &&
                    doctor.Specialization.ToLower().Contains(specialization.ToLower()))
                .ToList();
        }
        public List<Doctor> GetFilteredDoctors()
        {
            return _filteredDoctors;
        }
    }
}
