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
        private DoctorService doctorService;
        private EmailMessageRepository _emailMessageRepository;
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
    }
}
