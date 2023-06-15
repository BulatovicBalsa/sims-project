using System;
using System.Collections.Generic;

namespace Hospital.Core.PatientHealthcare.Models;

public enum HealthConditionType
{
    Allergy,
    MedicalCondition
}

public class HealthConditionList
{
    public HealthConditionList(HealthConditionType type)
    {
        Type = type;
        Conditions = new List<string>();
    }

    public HealthConditionList(HealthConditionType type, List<string> conditions)
    {
        Type = type;
        Conditions = conditions;
    }

    public HealthConditionList()
    {
        Conditions = new List<string>();
    }

    public HealthConditionType Type { get; }
    public List<string> Conditions { get; set; }

    public void Add(string conditionToAdd)
    {
        conditionToAdd = conditionToAdd.Trim();

        if (string.IsNullOrEmpty(conditionToAdd)) throw new ArgumentException($"{Type} name can't be empty");
        if (Conditions.Contains(conditionToAdd))
            throw new ArgumentException($"{conditionToAdd} already exists in medical record");
        Conditions.Add(conditionToAdd);
    }

    public void Delete(string selectedCondition)
    {
        if (!Conditions.Contains(selectedCondition))
            throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
        Conditions.Remove(selectedCondition);
    }

    public void Update(string selectedCondition, string updatedCondition)
    {
        updatedCondition = updatedCondition.Trim();
        var indexToUpdate = Conditions.IndexOf(selectedCondition);
        if (indexToUpdate == -1)
            throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
        if (string.IsNullOrEmpty(updatedCondition)) throw new ArgumentException($"{Type} name can't be empty");
        if (Conditions.Contains(updatedCondition))
            throw new ArgumentException($"{updatedCondition} already exist in this patient's medical record");
        Conditions[indexToUpdate] = updatedCondition;
    }
}