using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Nurse
{
    public class Nurse : Person
    {
        public Nurse() : base() { }

        public Nurse(string firstName, string lastName, string jmbg, string username, string password) : base(firstName, lastName, jmbg, username, password) {}
    }
}
