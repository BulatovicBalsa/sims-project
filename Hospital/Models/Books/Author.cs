﻿using System;

namespace Hospital.Models.Books
{
    public class Author
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public Author(string firstName, string lastName, DateTime birthDate)
        {
            Id = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public Author()
        {
            
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
