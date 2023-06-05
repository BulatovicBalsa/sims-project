using Hospital.DTOs;
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
        public List<Nurse> GetAll()
        {
            return _nurseRepository.GetAll();
        }
        public List<PersonDTO> GetNursesByFilter(string id, string searchText)
        {
            return _nurseRepository.GetNursesByFilter(id, searchText);
        }
    }
}
