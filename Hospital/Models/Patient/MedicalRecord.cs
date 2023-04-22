using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class MedicalRecord
    {
        private const int MIN_WEIGHT = 1;
        private const int MAX_WEIGHT = 200;
        private const int MIN_HEIGHT = 30;
        private const int MAX_HEIGHT = 220;
        public int Height { get; set; }
        public int Weight { get; set; }
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
            if (string.IsNullOrEmpty(updatedCondition))
            {
                throw new ArgumentException($"Medical condition name can't be empty");
            }
            if (MedicalHistory.Contains(updatedCondition))
            {
                throw new ArgumentException($"{updatedCondition} already exist in this patient's medical record");
            }
            MedicalHistory[indexToUpdate] = updatedCondition;
        }

        public void UpdateAllergy(string selectedAllergy, string updatedAllergy)
        {
            updatedAllergy = updatedAllergy.Trim();
            int indexToUpdate = Allergies.IndexOf(selectedAllergy);
            if (indexToUpdate == -1)
            {
                throw new ArgumentException($"{selectedAllergy} doesn't exist in this patient's medical record");
            }
            if (string.IsNullOrEmpty(updatedAllergy))
            {
                throw new ArgumentException($"Allergy name can't be empty");
            }
            if (MedicalHistory.Contains(updatedAllergy))
            {
                throw new ArgumentException($"{updatedAllergy} already exist in this patient's medical record");
            }
            Allergies[indexToUpdate] = updatedAllergy;
        }

        public void DeleteCondition(string selectedCondition)
        {
            if (!MedicalHistory.Contains(selectedCondition))
            {
                throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
            }
            MedicalHistory.Remove(selectedCondition);
        }

        public void DeleteAllergy(string selectedAllergy)
        {
            if (!Allergies.Contains(selectedAllergy))
            {
                throw new ArgumentException($"{selectedAllergy} doesn't exist in this patient's medical record");
            }
            Allergies.Remove(selectedAllergy);
        }

        public void ChangeWeight(int newWeight)
        {
            if (newWeight >= MIN_WEIGHT && newWeight <= MAX_WEIGHT)
            {
                Weight = newWeight;
            }
            else
            {
                throw new ArgumentException($"Weight must be between {MIN_WEIGHT} and {MAX_WEIGHT}");
            }
        }

        public void ChangeHeight(int newHeight)
        {
            if (newHeight >= MIN_HEIGHT && newHeight <= MAX_HEIGHT)
            {
                Height = newHeight;
            }
            else
            {
                throw new ArgumentException($"Height must be between {MIN_HEIGHT} and {MAX_HEIGHT}");
            }
        }

        public MedicalRecord DeepCopy()
        {
            MedicalRecord copy = new MedicalRecord(Height, Weight)
            {
                Allergies = new List<string>(Allergies),
                MedicalHistory = new List<string>(MedicalHistory),
                //Prescriptions = new List<Prescription>(Prescriptions.Select(p => p.DeepCopy()))
            };

            return copy;
        }
    }
}
