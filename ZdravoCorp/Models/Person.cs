using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class Person
    {
        static int freeId = 0;
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public Person(string firstName, string lastName, string jmbg)
        {
            Id = freeId;
            freeId++;
            FirstName = firstName;
            LastName = lastName;
            JMBG = jmbg;
        }
    }
}
