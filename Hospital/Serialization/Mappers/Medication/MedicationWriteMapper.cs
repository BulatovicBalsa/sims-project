using Hospital.Models.Patient;
using CsvHelper.Configuration;

namespace Hospital.Serialization.Mappers;

public sealed class MedicationWriteMapper : ClassMap<Medication>
{
    public MedicationWriteMapper()
    {
        Map(medication => medication.Id).Index(0);
        Map(medication => medication.Name).Index(1);
        Map(medication => medication.Stock).Index(2);
        Map(medication => medication.Allergens)
            .Convert(row => string.Join("|", row.Value.Allergens)).Index(3);
    }
}