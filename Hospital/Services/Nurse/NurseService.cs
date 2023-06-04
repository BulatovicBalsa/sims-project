using Hospital.Repositories.Nurse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class NurseService
    {
        private readonly NurseRepository _nurseRepository;
        public NurseService() 
        {
            _nurseRepository = NurseRepository.Instance;
        }
        public List<Nurse> GetAllNurses()
        {
            return _nurseRepository.GetAll();
        }
    }
}
