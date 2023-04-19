using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class Patient : Person
    {
        public const int MINIMUM_DAYS_TO_CHANGE_OR_DELETE_APPOINTMENT = 1;
        public const int MAX_CHANGES_OR_DELETES_LAST_30_DAYS = 4;
        public const int MAX_ALLOWED_APPOINTMENTS_LAST_30_DAYS = 8;
        public bool IsBlocked { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public Patient(string firstName, string lastName, string jmbg, string username, string password, MedicalRecord medicalRecord) : base(firstName, lastName, jmbg, username, password)
        {
            MedicalRecord = medicalRecord;
            IsBlocked = false;
        }

        public Patient() : base()
        {
            MedicalRecord = new MedicalRecord();
        }

        public Patient DeepCopy()
        {
            Patient copy = new Patient(FirstName, LastName, Jmbg, Profile.Username, Profile.Password, MedicalRecord.DeepCopy())
            {
                Id = this.Id,
                IsBlocked = this.IsBlocked
            };

            return copy;
        }

    }
}
