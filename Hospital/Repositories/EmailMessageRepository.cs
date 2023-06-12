using Hospital.Models;
using Hospital.Serialization;
using Hospital.Serialization.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public class EmailMessageRepository
    {
        private const string FilePath = "../../../Data/messages.csv";
        private static EmailMessageRepository? _instance;

        public static EmailMessageRepository Instance => _instance ??= new EmailMessageRepository();

        private EmailMessageRepository() { }
        public List<EmailMessage> GetAll()
        {
            return Serializer<EmailMessage>.FromCSV(FilePath, new EmailMessageReadMapper());
        }
        public List<EmailMessage> GetEmailMessagesByParticipantId(string id)
        {
            var allMessages = GetAll();
            return allMessages.Where(message => message.Sender.Id == id || message.Recipient.Id == id).ToList();
        }
        public void Add(EmailMessage message)
        {
            var allMessages = GetAll();
            allMessages.Add(message);
            Serializer<EmailMessage>.ToCSV(allMessages, FilePath, new EmailMessageWriteMapper());
        }
        public void Update(EmailMessage message)
        {
            var allMessages = GetAll();
            var indexToUpdate = allMessages.FindIndex(messageRecord => messageRecord.Id == message.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException($"Message with id {message.Id} was not found.");
            allMessages[indexToUpdate] = message;
            Serializer<EmailMessage>.ToCSV(allMessages, FilePath, new EmailMessageWriteMapper());
        }
        public void Delete(EmailMessage message)
        {
            var allMessages = GetAll();

            if (!allMessages.Remove(message))
                throw new KeyNotFoundException($"Message with id {message.Id} was not found.");

            Serializer<EmailMessage>.ToCSV(allMessages, FilePath, new EmailMessageWriteMapper());
        }
        public static void DeleteAll()
        {
            var emptyMessageList = new List<EmailMessage>();
            Serializer<EmailMessage>.ToCSV(emptyMessageList, FilePath);
        }

        public List<EmailMessage> GetReceivedEmailMessagesByParticipantId(string id)
        {
            var allMessages = GetAll();
            return allMessages.Where(message => message.Recipient.Id == id).ToList();
        }

        public List<EmailMessage> GetSentMessagesByParticipant(string id)
        {
            var allMessages = GetAll();
            return allMessages.Where(message => message.Sender.Id == id).ToList();
        }
    }
}
