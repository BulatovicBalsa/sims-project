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

        public DoctorSearchService()
        {
            _doctorRepository = DoctorRepository.Instance;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAll();
        }
    }
}
