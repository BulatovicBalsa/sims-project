using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Models.Patient;

public class HospitalTreatmentReferral
{
    public HospitalTreatmentReferral()
    {
        Prescriptions = new List<Prescription>();
        AdditionalTests = new List<string>();
    }

    public HospitalTreatmentReferral(List<Prescription> prescriptions, int duration, List<string> additionalTests)
    {
        Prescriptions = prescriptions;
        Duration = duration;
        AdditionalTests = additionalTests;
    }

    public List<Prescription> Prescriptions { get; set; }
    public int Duration { get; set; }
    public List<string> AdditionalTests { get; set; }
    public DateTime? Addmisson { get; set; }
    public DateTime? Release { get; set; }
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{Duration};");

        for (var i = 0; i < Prescriptions.Count; i++)
        {
            sb.Append(Prescriptions[i]?.ToString("$"));
            if (i < Prescriptions.Count - 1)
                sb.Append('#');
        }

        sb.Append(';');

        for (var i = 0; i < AdditionalTests.Count; i++)
        {
            sb.Append(AdditionalTests[i]);
            if (i < AdditionalTests.Count - 1)
                sb.Append('#');
        }

        return sb.ToString();
    }
}