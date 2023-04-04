using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models.Patients
{
    public class Patient : Person
    {
        public Patient(string firstName, string lastName, string jmbg, string username, string password) : base(firstName, lastName, jmbg, username, password) { }
    }
}
