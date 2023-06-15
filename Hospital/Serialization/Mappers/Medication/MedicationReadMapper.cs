using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace Hospital.Serialization.Mappers.Medication;

public sealed class MedicationReadMapper : ClassMap<Core.Pharmacy.Models.Medication>
{
    public MedicationReadMapper()
    {
        Map(medication => medication.Id).Index(0);
        Map(medication => medication.Name).Index(1);
        Map(medication => medication.Stock).Index(2);
        Map(medication => medication.Allergens).Index(3)
            .Convert(row => SplitColumnValues(row.Row.GetField<string>("Allergens")));
    }

    private static List<string> SplitColumnValues(string? columnValue)
    {
        var allergens = columnValue?.Split("|").ToList() ?? new List<string>();
        if (allergens.Count == 0) return allergens;
        return string.IsNullOrEmpty(allergens[0]) ? new List<string>() : allergens;
    }
}