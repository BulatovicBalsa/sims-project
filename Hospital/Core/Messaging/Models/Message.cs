using System;
using Hospital.Core.Accounts.DTOs;

namespace Hospital.Core.Messaging.Models;

public abstract class Message
{
    public Message(PersonDTO sender, PersonDTO recipient, string text, DateTime timestamp)
    {
        Id = Guid.NewGuid().ToString();
        Sender = sender;
        Recipient = recipient;
        Text = text;
        Timestamp = timestamp;
    }

    public Message(PersonDTO sender, PersonDTO recipient, string text)
    {
        Id = Guid.NewGuid().ToString();
        Sender = sender;
        Recipient = recipient;
        Text = text;
        Timestamp = DateTime.Now;
    }

    public Message()
    {
    }

    public string Id { get; set; }
    public PersonDTO Sender { get; set; }
    public PersonDTO Recipient { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}