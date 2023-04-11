using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Nurse;
using Hospital.Repositories.Patient;

namespace Hospital.Services
{
    public class LoginService
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly NurseRepository _nurseRepository;
        private readonly PatientRepository _patientRepository;

        public Person? LoggedUser { get; set; }

        public LoginService()
        {
            _doctorRepository = new DoctorRepository();
            _nurseRepository = new NurseRepository();
            _patientRepository = new PatientRepository();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            LoggedUser = _doctorRepository.GetByUsername(credential.UserName);
            if(LoggedUser != null && LoggedUser.Profile.Password == credential.Password)
            {
                return true;
            }

            LoggedUser = _nurseRepository.GetByUsername(credential.UserName);
            if (LoggedUser != null && LoggedUser.Profile.Password == credential.Password)
            {
                return true;
            }

            LoggedUser = _patientRepository.GetByUsername(credential.UserName);
            if (LoggedUser != null && LoggedUser.Profile.Password == credential.Password)
            {
                return true;
            }

            return false;
        }
    }
}
