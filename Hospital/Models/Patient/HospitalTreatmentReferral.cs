using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hospital.Models.Patient;

public class HospitalTreatmentReferral
{
    public HospitalTreatmentReferral()
    {
        Id = Guid.NewGuid().ToString();
        Prescriptions = new List<Prescription>();
        AdditionalTests = new List<string>();
    }

    public HospitalTreatmentReferral(List<Prescription> prescriptions, int duration, List<string> additionalTests, DateTime? admission=null, DateTime? release=null, string? roomId=null)
    {
        Id = Guid.NewGuid().ToString();
        Prescriptions = prescriptions;
        Duration = duration;
        AdditionalTests = additionalTests;
        Admission = admission;
        Release = release;
        RoomId = roomId;
    }

    public string Id { get; set; }
    public List<Prescription> Prescriptions { get; set; }
    public int Duration { get; set; }
    public List<string> AdditionalTests { get; set; }
    public DateTime? Admission { get; set; }
    public DateTime? Release { get; set; }
    public string? RoomId { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{Id};");
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
        sb.Append($";{RoomId}");

        return sb.ToString();
    }

    public void Accommodate(string roomId)
    {
        Admission = DateTime.Now;
        RoomId = roomId;
    }

    internal bool IsActive()
    {
        if (Admission is null) return false;
        return Release is null;
    }

    public bool Equals(HospitalTreatmentReferral? otherReferral)
    {
        if (otherReferral == null) return false;
        return Id == otherReferral.Id;
    }
}