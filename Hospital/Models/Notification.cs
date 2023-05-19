using System;

namespace Hospital.Models;

public class Notification
{
    public string Id { get; set; }
    public string ForId { get; set; }
    public string Message { get; set; }
    public bool Sent { get; set; }
    public Prescription? Prescription { get; set; }
    public DateTime? NotifyTime { get; set; }

    public Notification()
    {
        Id = Guid.NewGuid().ToString();
        ForId = "";
        Message = "";
        Sent = false;
    }
    public Notification(string forId, string message)
    {
        Id = Guid.NewGuid().ToString();
        ForId = forId;
        Message = message;
        Sent = false;
    }
    public Notification(Patient patient, string message, Prescription prescription)
    {
        Id = Guid.NewGuid().ToString();
        ForId = patient.Id;
        Message = message;
        Sent = false;
        Prescription = prescription;
        NotifyTime = CalculateNotifyTime(prescription.MedicationTiming);
    }
    private DateTime CalculateNotifyTime(MedicationTiming timing)
    {
        DateTime mealTime = DateTime.Now.Date.AddHours(12);
        TimeSpan durationBeforeMeal = TimeSpan.FromMinutes(30);
        DateTime anyTimeNotification = DateTime.Now.Date.AddHours(10);

        switch (timing)
        {
            case MedicationTiming.DuringMeal:
            case MedicationTiming.BeforeMeal:
                return mealTime - durationBeforeMeal;
            case MedicationTiming.AfterMeal:
                return mealTime + durationBeforeMeal;
            case MedicationTiming.Anytime:
            default:
                return anyTimeNotification;
        }
    }
}
