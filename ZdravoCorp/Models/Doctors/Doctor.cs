using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models.Doctors
{
    public class Doctor: Person
    {
        private const int DAYS_TO_CHECK = 3;

        public Doctor(string firstName, string lastName, string jmbg, string username, string password):base(firstName, lastName, jmbg, username, password) { }
    }
}
