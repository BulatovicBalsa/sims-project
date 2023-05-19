﻿using System;

namespace Hospital.Models.Patient;

public enum MedicationTiming
{
    DuringMeal,
    AfterMeal,
    BeforeMeal,
    Anytime
}

public class Prescription
{
    public Prescription()
    {
        Medication = new Medication();
    }

    public Prescription(Medication medication,int amount, int dailyUsage, MedicationTiming medicationTiming)
    {
        Medication = medication;
        Amount = amount;
        DailyUsage = dailyUsage;
        MedicationTiming = medicationTiming;
    }

    public int Amount { get; set; }
    public int DailyUsage { get; set; }
    public MedicationTiming MedicationTiming { get; set; }
    public Medication Medication { get; set; }

    public Prescription DeepCopy()
    {
        return new Prescription(Medication, Amount, DailyUsage, MedicationTiming);
    }

    public override string ToString()
    {
        return $"{Medication.Id};{Amount};{DailyUsage};{MedicationTiming}";
    }
    public string ToString(string separator)
    {
        return ToString().Replace(";", separator);
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Prescription objAsPrescription) return false;
        return objAsPrescription.Amount == Amount &&
            objAsPrescription.DailyUsage == DailyUsage &&
            objAsPrescription.MedicationTiming == MedicationTiming &&
            objAsPrescription.Medication.Equals(Medication);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}