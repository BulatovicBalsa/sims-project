using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models.Patients
{
    public class Patient : Person
    {
        public const int MINIMUM_DAYS_TO_CHANGE_OR_DELETE_APPOINTMENT = 1;
        public const int MAX_CHANGES_OR_DELETES_LAST_30_DAYS = 4;
        public const int MAX_ALLOWED_APPOINTMENTS_LAST_30_DAYS = 8;
        public bool IsBlocked { get; set; }
        public Patient(string firstName, string lastName, string jmbg, string username, string password) : base(firstName, lastName, jmbg, username, password) 
        {
            IsBlocked = false;
        }
       
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Patient? other = obj as Patient;
            if (other == null) return false;
            return Id == other.Id;
        }

    }

}
