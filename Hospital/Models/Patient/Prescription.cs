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
}