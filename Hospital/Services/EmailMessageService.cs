using Hospital.DTOs;
using Hospital.Injectors;
using Hospital.Models;
using Hospital.Models.Patient;
using Hospital.Repositories;
using Hospital.Repositories.Patient;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class EmailMessageService
    {
        private DoctorService _doctorService;
        private NurseService _nurseService;
        private EmailMessageRepository _emailMessageRepository = new EmailMessageRepository(SerializerInjector.CreateInstance<ISerializer<EmailMessage>>());
        public EmailMessageService()
        {
            _doctorService = new DoctorService();
            _nurseService = new NurseService();
        }
        public List<EmailMessage> GetAllEmailMessagesByParticipant(string id)
        {
         return _emailMessageRepository.GetEmailMessagesByParticipantId(id);
        }

        public List<EmailMessage> GetReceivedEmailMessagesByParticipant(string id)
        {
            return _emailMessageRepository.GetReceivedEmailMessagesByParticipantId(id);
        }

        public List<EmailMessage> GetSentEmailMessagesByParticipant(string id)
        {
            return _emailMessageRepository.GetSentMessagesByParticipant(id);
        }
        public List<PersonDTO> GetMedicalStaffByFilter(string id, string searchText)
        {
            var filteredDoctors = _doctorService.GetDoctorsAsPersonDTOsByFilter(id, searchText);
            var filteredNurses = _nurseService.GetNursesAsPersonDTOsByFilter(id, searchText);
            return ConcatenateDoctorsAndNurses(filteredDoctors, filteredNurses);
        }

        public void SendMessage(EmailMessage message)
        {
            _emailMessageRepository.Add(message);
        }

        private List<PersonDTO> ConcatenateDoctorsAndNurses(List<PersonDTO> filteredDoctors, List<PersonDTO> filteredNurses)
        {
            var medicalStaff = filteredDoctors.Concat(filteredNurses).ToList();
            return medicalStaff;
        }
    }
}
