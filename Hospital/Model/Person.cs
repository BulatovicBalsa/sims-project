using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class Person
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Jmbg { get; set; }
        public Profile Profile { get; set; }

        public Person(string firstName, string lastName, string jmbg, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Jmbg = jmbg;
            Profile = new Profile(username, password);
        }
    }
}
