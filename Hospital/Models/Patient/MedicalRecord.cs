using System;
using System.Collections.Generic;

namespace Hospital.Models.Patient;

public class MedicalRecord
{
    private const int MinWeight = 1;
    private const int MaxWeight = 200;
    private const int MinHeight = 30;
    private const int MaxHeight = 220;
    //public List<Prescription> Prescriptions { get; set; }

    public MedicalRecord()
    {
        Weight = 0;
        Height = 0;
        Allergies = new HealthCondition(HealthConditionType.Allergy);
        MedicalHistory = new HealthCondition(HealthConditionType.MedicalCondition);
        //Prescriptions = new List<Prescription>();
    }

    public MedicalRecord(int height, int weight)
    {
        Height = height;
        Weight = weight;
        Allergies = new HealthCondition(HealthConditionType.Allergy);
        MedicalHistory = new HealthCondition(HealthConditionType.MedicalCondition);
        //Prescriptions = new List<Prescription>();
    }

    public MedicalRecord(int height, int weight, List<string> allergies, List<string> medicalHistory)
    {
        Height = height;
        Weight = weight;
        Allergies = new HealthCondition(HealthConditionType.Allergy, allergies);
        MedicalHistory = new HealthCondition(HealthConditionType.MedicalCondition, medicalHistory);
        //Prescriptions = prescriptions;
    }

    public int Height { get; set; }
    public int Weight { get; set; }
    public HealthCondition Allergies { get; set; }
    public HealthCondition MedicalHistory { get; set; }

    public void ChangeWeight(int newWeight)
    {
        if (newWeight is >= MinWeight and <= MaxWeight)
            Weight = newWeight;
        else
            throw new ArgumentException($"Weight must be between {MinWeight} and {MaxWeight}");
    }

    public void ChangeHeight(int newHeight)
    {
        if (newHeight is >= MinHeight and <= MaxHeight)
            Height = newHeight;
        else
            throw new ArgumentException($"Height must be between {MinHeight} and {MaxHeight}");
    }

    public MedicalRecord DeepCopy()
    {
        var copy = new MedicalRecord(Height, Weight)
        {
            Allergies = new HealthCondition(Allergies.Type, Allergies.Conditions),
            MedicalHistory = new HealthCondition(MedicalHistory.Type, MedicalHistory.Conditions)
            //Prescriptions = new List<Prescription>(Prescriptions.Select(p => p.DeepCopy()))
        };

        return copy;
    }
}