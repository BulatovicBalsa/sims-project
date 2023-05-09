using System;
using System.Collections.Generic;

namespace Hospital.Models.Patient;

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

    public int Height { get; set; }
    public int Weight { get; set; }
    public List<string> Allergies { get; set; }
    public List<string> MedicalHistory { get; set; }

    public void AddAllergy(string allergyToAdd)
    {
        AddHealthCondition(allergyToAdd, HealthConditionType.Allergy);
    }

    public void AddMedicalCondition(string medicalConditionToAdd)
    {
        AddHealthCondition(medicalConditionToAdd, HealthConditionType.MedicalCondition);
    }

    public void UpdateMedicalCondition(string selectedMedicalCondition, string updatedMedicalCondition)
    {
        UpdateHealthCondition(selectedMedicalCondition, updatedMedicalCondition, HealthConditionType.MedicalCondition);
    }

    public void UpdateAllergy(string selectedAllergy, string updatedAllergy)
    {
        UpdateHealthCondition(selectedAllergy, updatedAllergy, HealthConditionType.Allergy);
    }

    public void DeleteMedicalCondition(string selectedMedicalCondition)
    {
        DeleteHealthCondition(selectedMedicalCondition, HealthConditionType.MedicalCondition);
    }

    public void DeleteAllergy(string selectedAllergy)
    {
        DeleteHealthCondition(selectedAllergy, HealthConditionType.Allergy);
    }

    private void AddHealthCondition(string conditionToAdd, HealthConditionType conditionType)
    {
        var healthConditionList = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;
        conditionToAdd = conditionToAdd.Trim();

        if (string.IsNullOrEmpty(conditionToAdd)) throw new ArgumentException($"{conditionType} name can't be empty");
        if (healthConditionList.Contains(conditionToAdd))
            throw new ArgumentException($"{conditionToAdd} already exists in medical record");
        healthConditionList.Add(conditionToAdd);
    }

    private void DeleteHealthCondition(string selectedCondition, HealthConditionType conditionType)
    {
        var healthConditionList = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

        if (!healthConditionList.Contains(selectedCondition))
            throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
        healthConditionList.Remove(selectedCondition);
    }

    private void UpdateHealthCondition(string selectedCondition, string updatedCondition,
        HealthConditionType conditionType)
    {
        var healthConditionList = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

        updatedCondition = updatedCondition.Trim();
        var indexToUpdate = healthConditionList.IndexOf(selectedCondition);
        if (indexToUpdate == -1)
            throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
        if (string.IsNullOrEmpty(updatedCondition)) throw new ArgumentException($"{conditionType} name can't be empty");
        if (healthConditionList.Contains(updatedCondition))
            throw new ArgumentException($"{updatedCondition} already exist in this patient's medical record");
        healthConditionList[indexToUpdate] = updatedCondition;
    }

    public void ChangeWeight(int newWeight)
    {
        if (newWeight is >= MIN_WEIGHT and <= MAX_WEIGHT)
            Weight = newWeight;
        else
            throw new ArgumentException($"Weight must be between {MIN_WEIGHT} and {MAX_WEIGHT}");
    }

    public void ChangeHeight(int newHeight)
    {
        if (newHeight is >= MIN_HEIGHT and <= MAX_HEIGHT)
            Height = newHeight;
        else
            throw new ArgumentException($"Height must be between {MIN_HEIGHT} and {MAX_HEIGHT}");
    }

    public MedicalRecord DeepCopy()
    {
        var copy = new MedicalRecord(Height, Weight)
        {
            Allergies = new List<string>(Allergies),
            MedicalHistory = new List<string>(MedicalHistory)
            //Prescriptions = new List<Prescription>(Prescriptions.Select(p => p.DeepCopy()))
        };

        return copy;
    }
}