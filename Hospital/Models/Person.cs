using System;
using System.Diagnostics;

namespace Hospital.Models
{
    public abstract class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Jmbg { get; set; }
        public Profile Profile { get; set; }
        protected Person()
        {
            Id = Guid.NewGuid().ToString();
            FirstName = "";
            LastName = "";
            Jmbg = "";
            Profile = new Profile();
        }

        protected Person(string firstName, string lastName, string jmbg, string username, string password)
        {
            Id = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            Jmbg = jmbg;
            Profile = new Profile(username, password);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Person objAsPerson) return false;
            return Id == objAsPerson.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
