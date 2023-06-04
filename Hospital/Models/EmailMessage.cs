using Hospital.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class EmailMessage : Message
    {
        public string Subject { get; set; }

        public EmailMessage(PersonDTO sender, PersonDTO recipient, string subject, string from, string to, string text, DateTime timestamp)
            : base(sender, recipient, text, timestamp)
        {
            Subject = subject;
        }

        public EmailMessage()
        {
        }
    }
}
