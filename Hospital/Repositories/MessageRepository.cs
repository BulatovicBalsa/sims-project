using Hospital.Models;
using Hospital.Serialization;
using Hospital.Serialization.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories
{
    public class MessageRepository
    {
        private const string FilePath = "../../Data/messages.csv";
        private static MessageRepository? _instance;

        public static MessageRepository Instance => _instance ??= new MessageRepository();

        private MessageRepository() { }
        public List<Message> GetAll()
        {
            return Serializer<Message>.FromCSV(FilePath, new MessageReadMapper());
        }
        public List<Message> GetMessagesByParticipantId(string id)
        {
            var allMessages = GetAll();
            return allMessages.Where(message => message.Sender.Id == id || message.Recipient.Id == id).ToList();
        }
        public void Add(Message message)
        {
            var allMessages = GetAll();
            allMessages.Add(message);
            Serializer<Message>.ToCSV(allMessages, FilePath, new MessageWriteMapper());
        }
        public void Update(Message message)
        {
            var allMessages = GetAll();
            var indexToUpdate = allMessages.FindIndex(messageRecord => messageRecord.Id == message.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException($"Message with id {message.Id} was not found.");
            allMessages[indexToUpdate] = message;
            Serializer<Message>.ToCSV(allMessages, FilePath, new MessageWriteMapper());
        }
        public void Delete(Message message)
        {
            var allMessages = GetAll();

            if (!allMessages.Remove(message))
                throw new KeyNotFoundException($"Message with id {message.Id} was not found.");

            Serializer<Message>.ToCSV(allMessages, FilePath, new MessageWriteMapper());
        }
        public static void DeleteAll()
        {
            var emptyMessageList = new List<Message>();
            Serializer<Message>.ToCSV(emptyMessageList, FilePath);
        }
    }
}
