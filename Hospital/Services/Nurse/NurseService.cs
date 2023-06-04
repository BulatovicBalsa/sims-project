using Hospital.Models.Nurse;
using Hospital.Repositories.Nurse;
using System.Collections.Generic;

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
