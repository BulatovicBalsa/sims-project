using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Models.Services
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

        public void Add(Doctor item)
        {
            _doctorRepository.Add(item);
        }

        public void Update(Doctor item)
        {
            _doctorRepository.Update(item);
        }

        public void Delete(Doctor item)
        {
            _doctorRepository.Delete(item);
        }
    }
}
