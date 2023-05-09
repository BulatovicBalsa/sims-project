using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace Hospital.Serialization.Mappers.Patient;

using Hospital.Models.Patient;

public sealed class PatientReadMapper : ClassMap<Patient>
{
    public PatientReadMapper()
    {
        Map(patient => patient.Id).Index(0);
        Map(patient => patient.FirstName).Index(1);
        Map(patient => patient.LastName).Index(2);
        Map(patient => patient.Jmbg).Index(3);
        Map(patient => patient.Profile.Username).Index(4);
        Map(patient => patient.Profile.Password).Index(5);
        Map(patient => patient.MedicalRecord.Height).Index(6);
        Map(patient => patient.MedicalRecord.Weight).Index(7);
        Map(patient => patient.MedicalRecord.Allergies).Index(8)
            .Convert(row => SplitColumnValues(row.Row.GetField<string>("Allergies")));
        Map(patient => patient.MedicalRecord.MedicalHistory).Index(9)
            .Convert(row => SplitColumnValues(row.Row.GetField<string>("MedicalHistory")));
        Map(patient => patient.IsBlocked).Index(10);
    }

    private List<string> SplitColumnValues(string? columnValue)
    {
        return columnValue?.Split("|").ToList() ?? new List<string>();
    }
}