using System;

namespace Library.Models.Books
{
    public class Publisher
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public Publisher(string name, string phoneNumber)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
