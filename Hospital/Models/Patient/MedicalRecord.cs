using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class MedicalRecord
    {
        public int Weight { get; set; }
        public int Height { get; set; }
        public List<string> Allergies { get; set; }
        public List<string> MedicalHistory { get; set; }
        public List<Prescription> Prescriptions { get; set; }

        // TODO: Add a list of Examinations, each Examination has an Anamnesis attached to it

        public MedicalRecord()
        {
            Weight = -1;
            Height = -1;
            Allergies = new List<string>();
            MedicalHistory = new List<string>();
            Prescriptions = new List<Prescription>();
        }

        public MedicalRecord(int weight, int height)
        {
            Weight = weight;
            Height = height;
            Allergies = new List<string>();
            MedicalHistory = new List<string>();
            Prescriptions = new List<Prescription>();
        }
    }
}
