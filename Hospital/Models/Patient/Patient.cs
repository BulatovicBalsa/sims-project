using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class Patient : Person
    {
        public MedicalRecord MedicalRecord { get; set; }

        public Patient() : base()
        {
            MedicalRecord = new MedicalRecord();
        }

        public Patient(string firstName, string lastName, string jmbg, string username, string password, MedicalRecord medicalRecord) : base(firstName, lastName, jmbg, username, password)
        {
            MedicalRecord = medicalRecord;
        }
    }
}
