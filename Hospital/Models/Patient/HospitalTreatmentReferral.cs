using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hospital.Models.Patient;

public class HospitalTreatmentReferral
{
    public HospitalTreatmentReferral()
    {
        Prescriptions = new List<Prescription>();
        AdditionalTests = new List<string>();
    }

    public HospitalTreatmentReferral(List<Prescription> prescriptions, int duration, List<string> additionalTests, DateTime? admission=null, DateTime? release=null)
    {
        Prescriptions = prescriptions;
        Duration = duration;
        AdditionalTests = additionalTests;
        Admission = admission;
        Release = release;
    }

    public List<Prescription> Prescriptions { get; set; }
    public int Duration { get; set; }
    public List<string> AdditionalTests { get; set; }
    public DateTime? Admission { get; set; }
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

        sb.Append($";{Admission:yyyy-MM-dd HH:mm:ss};{Release:yyyy-MM-dd HH:mm:ss}");

        return sb.ToString();
    }

    internal bool IsActive()
    {
        if (Admission is null) return false;
        return Release is null;
    }

    public bool Equals(HospitalTreatmentReferral? otherReferral)
    {
        if (otherReferral == null) return false;
        var areTestsEqual = otherReferral.AdditionalTests.SequenceEqual(AdditionalTests);
        var arePrescriptionsEqual = otherReferral.Prescriptions.SequenceEqual(Prescriptions);
        return otherReferral.Admission == Admission && otherReferral.Release == Release &&
               otherReferral.Duration == Duration && arePrescriptionsEqual && areTestsEqual;
    }
}