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
    }

    public Prescription(int amount, int dailyUsage, MedicationTiming medicationTiming)
    {
        Amount = amount;
        DailyUsage = dailyUsage;
        MedicationTiming = medicationTiming;
    }

    public int Amount { get; set; }
    public int DailyUsage { get; set; }
    public MedicationTiming MedicationTiming { get; set; }

    public Prescription DeepCopy()
    {
        return new Prescription(Amount, DailyUsage, MedicationTiming);
    }
}