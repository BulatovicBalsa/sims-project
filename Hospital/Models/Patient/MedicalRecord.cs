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
        //public List<Prescription> Prescriptions { get; set; }

        public MedicalRecord()
        {
            Weight = 0;
            Height = 0;
            Allergies = new List<string>();
            MedicalHistory = new List<string>();
            //Prescriptions = new List<Prescription>();
        }

        public MedicalRecord(int height, int weight)
        {
            Height = height;
            Weight = weight;
            Allergies = new List<string>();
            MedicalHistory = new List<string>();
            //Prescriptions = new List<Prescription>();
        }

        public MedicalRecord(int height, int weight, List<string> allergies, List<string> medicalHistory)
        {
            Height = height;
            Weight = weight;
            Allergies = allergies;
            MedicalHistory = medicalHistory;
            //Prescriptions = prescriptions;
        }

        public void AddAllergy(string allergyToAdd)
        {
            allergyToAdd = allergyToAdd.Trim();
            if (string.IsNullOrEmpty(allergyToAdd)) throw new ArgumentException("Allergy name can't be empty");
            if (Allergies.Contains(allergyToAdd)) throw new ArgumentException($"{allergyToAdd} already exists in medical record");
            Allergies.Add(allergyToAdd);
        }

        public void AddConidition(string conditionToAdd)
        {
            conditionToAdd = conditionToAdd.Trim();
            if (string.IsNullOrEmpty(conditionToAdd)) throw new ArgumentException("Medical condition name can't be empty");
            if (MedicalHistory.Contains(conditionToAdd)) throw new ArgumentException($"{conditionToAdd} already exists in medical record");
            MedicalHistory.Add(conditionToAdd);
        }

        public void UpdateCondition(string selectedCondition, string updatedCondition)
        {
            int indexToUpdate = MedicalHistory.IndexOf(selectedCondition);
            if (indexToUpdate == -1) {
                throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
            }
            if (MedicalHistory.Contains(updatedCondition))
            {
                throw new ArgumentException($"{updatedCondition} already exist in this patient's medical record");
            }
            MedicalHistory[indexToUpdate] = updatedCondition;
        }

        public void UpdateAllergy(string selectedAllergy, string updatedAllergy)
        {
            int indexToUpdate = Allergies.IndexOf(selectedAllergy);
            if (indexToUpdate == -1)
            {
                throw new ArgumentException($"{selectedAllergy} doesn't exist in this patient's medical record");
            }
            if (MedicalHistory.Contains(updatedAllergy))
            {
                throw new ArgumentException($"{updatedAllergy} already exist in this patient's medical record");
            }
            Allergies[indexToUpdate] = updatedAllergy;
        }
    }
}
