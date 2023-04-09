using System;

namespace Hospital.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public Profile Profile { get; set; }
        public Person(string firstName, string lastName, string jmbg, string username, string password)
        {
            Id = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            JMBG = jmbg;
            Profile = new Profile(username, password);
        }

    }
}