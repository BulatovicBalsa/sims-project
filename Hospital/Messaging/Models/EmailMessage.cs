using System;
using Hospital.DTOs;

namespace Hospital.Messaging.Models;

public class EmailMessage : Message
{
    public EmailMessage(PersonDTO sender, PersonDTO recipient, string subject, string text, DateTime timestamp)
        : base(sender, recipient, text, timestamp)
    {
        Subject = subject;
    }

    public EmailMessage(PersonDTO sender, PersonDTO recipient, string subject, string text)
        : base(sender, recipient, text)
    {
        Subject = subject;
    }

    public EmailMessage()
    {
    }

    public string Subject { get; set; }
}