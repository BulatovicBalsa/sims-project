using System;

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

    public Prescription(Medication medication, int amount, int dailyUsage, MedicationTiming medicationTiming, string doctorId)
    {
        Medication = medication;
        Amount = amount;
        DailyUsage = dailyUsage;
        MedicationTiming = medicationTiming;
        IssuedDate = DateTime.Now;
        DoctorId = doctorId;
    }

    public int Amount { get; set; }
    public int DailyUsage { get; set; }
    public MedicationTiming MedicationTiming { get; set; }
    public Medication Medication { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime? LastUsed { get; set; }
    public string DoctorId { get; set; }

    public string ListBoxString =>
        $"{Medication.Name}, Amount: {Amount}, Daily Usage: {DailyUsage}, Timing: {MedicationTiming}";

    public Prescription DeepCopy()
    {
        return new Prescription(Medication, Amount, DailyUsage, MedicationTiming, DoctorId);
    }

    public override string ToString()
    {
        return $"{Medication.Id};{Amount};{DailyUsage};{MedicationTiming};{IssuedDate:yyyy-MM-dd HH:mm:ss};{LastUsed:yyyy-MM-dd HH:mm:ss};{DoctorId}";
    }

    public string ToString(string separator)
    {
        return ToString().Replace(";", separator);
    }

    public string ComboBoxString => $"{Medication.Name} {Amount}/{DailyUsage} {IssuedDate}";

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

    public bool CanBeDispensed()
    {
        if (LastUsed == null)
        {
            return true;
        }

        var daysLasting = Amount / DailyUsage;
        var daysSinceLastDose = (DateTime.Now - (DateTime)LastUsed).TotalDays;

        return daysSinceLastDose >= daysLasting - 1;
    }
}