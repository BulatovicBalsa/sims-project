using Hospital.Models.Patient;
using System;
using PatientClass = Hospital.Models.Patient.Patient;

namespace Hospital.Models;

public class Notification
{
    private const int MealHour = 12;
    private const int AnytimeNotificationHour= 10;
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
        NotifyTime=DateTime.MinValue;
    }
    public Notification(PatientClass patient, string message, Prescription prescription)
    {
        Id = Guid.NewGuid().ToString();
        ForId = patient.Id;
        Message = message;
        Sent = false;
        Prescription = prescription;
        NotifyTime = CalculateNotifyTime(patient.NotificationTime,prescription.MedicationTiming);
    }
    private DateTime CalculateNotifyTime(int NotificationTime,MedicationTiming timing)
    {
        DateTime mealTime = DateTime.Now.Date.AddHours(MealHour);
        TimeSpan durationBeforeMeal = TimeSpan.FromMinutes(NotificationTime);
        DateTime anyTimeNotification = DateTime.Now.Date.AddHours(AnytimeNotificationHour);

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
