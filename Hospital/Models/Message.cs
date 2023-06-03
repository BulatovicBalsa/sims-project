using Hospital.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Hospital.Models
{
    public class Message
    {
        public string Id { get; set; }
        public PersonDTO Sender { get; set; }
        public PersonDTO Recipient { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public Message(PersonDTO sender, PersonDTO recipient, string text, DateTime timestamp)
        {
            Id = Guid.NewGuid().ToString();
            Sender = sender;
            Recipient = recipient;
            Text = text;
            Timestamp = timestamp;
        }

        public Message() {}
    }

}
