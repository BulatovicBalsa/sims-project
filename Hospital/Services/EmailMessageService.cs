using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
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
        private EmailMessageRepository _emailMessageRepository;
        public EmailMessageService()
        {
            _doctorService = new DoctorService();
            _nurseService = new NurseService();
            _emailMessageRepository = EmailMessageRepository.Instance;
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
        public List<PersonDTO> GetFilteredMedicalStaff(string id)
        {
            var allDoctors = _doctorService.GetAll();
            var allNurses = _nurseService.GetAllNurses();

            var filteredDoctors = allDoctors.Where(doctor => doctor.Id != id);
            var filteredNurses = allNurses.Where(nurse => nurse.Id != id);

            var medicalStaff = filteredDoctors
            .Select(doctor => new PersonDTO(doctor.Id, doctor.FirstName, doctor.LastName, Role.Doctor))
            .Concat(filteredNurses
                    .Select(nurse => new PersonDTO(nurse.Id, nurse.FirstName, nurse.LastName, Role.Nurse)))
            .ToList();

            return medicalStaff;
        }

        public void SendMessage(EmailMessage message)
        {
            _emailMessageRepository.Add(message);
        }
    }
}
