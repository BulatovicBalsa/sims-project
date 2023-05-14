using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Hospital.Serialization.Mappers.Patient
{
    public sealed class MedicationWriteMapper : ClassMap<Medication>
    {
        public MedicationWriteMapper()
        {
            Map(medication => medication.Id).Index(0);
            Map(medication => medication.Allergens)
                .Convert(row => string.Join("|", row.Value.Allergens)).Index(1);
        }
    }
}
