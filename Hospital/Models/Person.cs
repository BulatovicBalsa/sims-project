using System;

namespace Hospital.Models
{
    public abstract class Person
    {
        public string Id { get; }
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
    }
}
