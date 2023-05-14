using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Hospital.Models.Patient;
using static Hospital.Serialization.Mappers.Patient.PatientReadMapper;

namespace Hospital.Serialization.Mappers.Patient
{
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
            return columnValue?.Split("|").ToList() ?? new List<string>();
        }
    }
}
