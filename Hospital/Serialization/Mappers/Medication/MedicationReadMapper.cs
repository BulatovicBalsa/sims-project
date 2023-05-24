﻿using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using Hospital.Models.Patient;

namespace Hospital.Serialization.Mappers;

public sealed class MedicationReadMapper : ClassMap<Medication>
{
    public MedicationReadMapper()
    {
        Map(medication => medication.Id).Index(0);
        Map(medication => medication.Name).Index(1);
        Map(medication => medication.Allergens).Index(2)
            .Convert(row => SplitColumnValues(row.Row.GetField<string>("Allergens")));
    }

    private static List<string> SplitColumnValues(string? columnValue)
    {
        var allergens = columnValue?.Split("|").ToList() ?? new List<string>();
        if (allergens.Count == 0) return allergens;
        return string.IsNullOrEmpty(allergens[0]) ? new List<string>() : allergens;
    }
}