using System.Collections.Generic;
using System.Linq;
using Hospital.Core.Accounts.DTOs;
using Hospital.Core.Messaging.Models;
using Hospital.Core.Messaging.Repositories;
using Hospital.Core.Workers.Services;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.Messaging.Services;

public class EmailMessageService
{
    private readonly DoctorService _doctorService;

    private readonly EmailMessageRepository _emailMessageRepository =
        new(SerializerInjector.CreateInstance<ISerializer<EmailMessage>>());

    private readonly NurseService _nurseService;

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