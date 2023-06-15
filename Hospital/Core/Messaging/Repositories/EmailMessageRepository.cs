using System.Collections.Generic;
using System.Linq;
using Hospital.Core.Messaging.Models;
using Hospital.Serialization;
using Hospital.Serialization.Mappers;

namespace Hospital.Core.Messaging.Repositories;

public class EmailMessageRepository
{
    private const string FilePath = "../../../Data/messages.csv";
    private readonly ISerializer<EmailMessage> _serializer;

    public EmailMessageRepository(ISerializer<EmailMessage> serializer)
    {
        _serializer = serializer;
    }

    public List<EmailMessage> GetAll()
    {
        return _serializer.Load(FilePath, new EmailMessageReadMapper());
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
        _serializer.Save(allMessages, FilePath, new EmailMessageWriteMapper());
    }

    public void Update(EmailMessage message)
    {
        var allMessages = GetAll();
        var indexToUpdate = allMessages.FindIndex(messageRecord => messageRecord.Id == message.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException($"Message with id {message.Id} was not found.");
        allMessages[indexToUpdate] = message;
        _serializer.Save(allMessages, FilePath, new EmailMessageWriteMapper());
    }

    public void Delete(EmailMessage message)
    {
        var allMessages = GetAll();

        if (!allMessages.Remove(message))
            throw new KeyNotFoundException($"Message with id {message.Id} was not found.");

        _serializer.Save(allMessages, FilePath, new EmailMessageWriteMapper());
    }

    public void DeleteAll()
    {
        var emptyMessageList = new List<EmailMessage>();
        _serializer.Save(emptyMessageList, FilePath);
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