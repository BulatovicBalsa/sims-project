using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public enum HealthConditionType
    {
        Allergy,
        MedicalCondition
    }

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
            addHealthCondition(allergyToAdd, HealthConditionType.Allergy);
        }

        public void AddMedicalConidition(string medicalConditionToAdd)
        {
            addHealthCondition(medicalConditionToAdd, HealthConditionType.MedicalCondition);
        }

        public void UpdateMedicalCondition(string selectedMedicalCondition, string updatedMedicalCondition)
        {
            updateHealthCondition(selectedMedicalCondition, updatedMedicalCondition, HealthConditionType.MedicalCondition);
        }

        public void UpdateAllergy(string selectedAllergy, string updatedAllergy)
        {
            updateHealthCondition(selectedAllergy, updatedAllergy, HealthConditionType.Allergy);
        }

        public void DeleteMedicalCondition(string selectedMedicalCondition)
        {
            deleteHealthCondition(selectedMedicalCondition, HealthConditionType.MedicalCondition);
        }

        public void DeleteAllergy(string selectedAllergy)
        {
            deleteHealthCondition(selectedAllergy, HealthConditionType.Allergy);
        }

        private void addHealthCondition(string conditionToAdd, HealthConditionType conditionType)
        {
            var healthConditionList = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;
            conditionToAdd = conditionToAdd.Trim();

            if (string.IsNullOrEmpty(conditionToAdd)) throw new ArgumentException($"{conditionType} name can't be empty");
            if (healthConditionList.Contains(conditionToAdd)) throw new ArgumentException($"{conditionToAdd} already exists in medical record");
            healthConditionList.Add(conditionToAdd);
            
        }

        private void deleteHealthCondition(string selectedCondition, HealthConditionType conditionType)
        {
            var healthConditionList = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

            if (!healthConditionList.Contains(selectedCondition))
            {
                throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
            }
            healthConditionList.Remove(selectedCondition);
        }

        private void updateHealthCondition(string selectedCondition, string updatedCondition, HealthConditionType conditionType)
        {
            var healthConditionList = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

            updatedCondition = updatedCondition.Trim();
            int indexToUpdate = healthConditionList.IndexOf(selectedCondition);
            if (indexToUpdate == -1)
            {
                throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
            }
            if (string.IsNullOrEmpty(updatedCondition))
            {
                throw new ArgumentException($"{conditionType} name can't be empty");
            }
            if (healthConditionList.Contains(updatedCondition))
            {
                throw new ArgumentException($"{updatedCondition} already exist in this patient's medical record");
            }
            healthConditionList[indexToUpdate] = updatedCondition;
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

        public string GetMedicalHistoryString()
        {
            return string.Join(", ", MedicalHistory);
        }
    }
}
