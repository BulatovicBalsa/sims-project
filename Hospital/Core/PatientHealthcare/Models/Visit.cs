using System;

namespace Hospital.Core.PatientHealthcare.Models;

public class Visit
{
    public Visit()
    {
        PatientId = "";
        BloodPressure = "";
        BodyTemperature = 0;
        Observations = "";
        Time = DateTime.Now;
    }

    public Visit(string patientId, string bloodPressure, float bodyTemperature, string observations, DateTime time)
    {
        PatientId = patientId;
        BloodPressure = bloodPressure;
        BodyTemperature = bodyTemperature;
        Observations = observations;
        Time = time;
    }

    public string PatientId { get; set; }
    public string BloodPressure { get; set; }
    public float BodyTemperature { get; set; }
    public string Observations { get; set; }
    public DateTime Time { get; set; }
}