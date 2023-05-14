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
            Map(patient => patient.Id).Index(0);
            Map(patient => patient.Allergens).Index(1)
                .Convert(row => SplitColumnValues(row.Row.GetField<string>("Allergens")));
        }
        private static List<string> SplitColumnValues(string? columnValue)
        {
            return columnValue?.Split("|").ToList() ?? new List<string>();
        }
    }
}
