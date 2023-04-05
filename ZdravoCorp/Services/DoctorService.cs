using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    public class DoctorService: IService<Doctor>
    {
        private readonly DoctorRepository _doctorRepository;

        public DoctorService(DoctorRepository DoctorRepository)
        {
            _doctorRepository = DoctorRepository;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor? GetById(int id)
        {
            return _doctorRepository.GetById(id);
        }

        public void Add(Doctor doctor)
        {
            _doctorRepository.Add(doctor);
        }

        public void Update(Doctor doctor, bool isPatient)
        {
            _doctorRepository.Update(doctor);
        }

        public void Delete(Doctor doctor, bool isPatient)
        {
            _doctorRepository.Delete(doctor);
        }
    }
}
