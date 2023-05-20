using CsvHelper.Configuration;

namespace Hospital.Serialization.Mappers.Patient;

using Hospital.Models.Patient;

public sealed class PatientWriteMapper : ClassMap<Patient>
{
    public PatientWriteMapper()
    {
        Map(patient => patient.Id).Index(0);
        Map(patient => patient.FirstName).Index(1);
        Map(patient => patient.LastName).Index(2);
        Map(patient => patient.Jmbg).Index(3);
        Map(patient => patient.Profile.Username).Index(4);
        Map(patient => patient.Profile.Password).Index(5);
        Map(patient => patient.MedicalRecord.Height).Index(6);
        Map(patient => patient.MedicalRecord.Weight).Index(7);
        Map(patient => patient.MedicalRecord.Allergies)
            .Convert(row => string.Join("|", row.Value.MedicalRecord.Allergies.Conditions)).Index(8);
        Map(patient => patient.MedicalRecord.MedicalHistory)
            .Convert(row => string.Join("|", row.Value.MedicalRecord.MedicalHistory.Conditions)).Index(9);
        Map(patient => patient.IsBlocked).Index(10);
        Map(patient => patient.Referrals).Index(11).Convert(row => string.Join("|", row.Value.Referrals)).Index(11); ;
    }
}