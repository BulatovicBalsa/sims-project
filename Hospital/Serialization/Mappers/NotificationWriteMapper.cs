using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Models;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Serialization.Mappers
{
    public class NotificationWriteMapper : ClassMap<Notification>
    {
        public NotificationWriteMapper()
        {
            Map(m => m.Id).Index(0);
            Map(m => m.ForId).Index(1);
            Map(m => m.Message).Index(2);
            Map(m => m.Sent).Index(3);
            Map(m => m.Prescription).Index(4).TypeConverter<PrescriptionTypeConverter>();
            Map(m => m.NotifyTime).Index(5).TypeConverter<NullableDateTimeConverter>();
        }
    }

    public class NullableDateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (DateTime.TryParse(text, out var result))
            {
                return result;
            }

            return null;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return string.Empty;
        }
    }
    public class PrescriptionTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var prescriptionArgs = text.Split(';');
            if (prescriptionArgs.Length != 6)
            {
                throw new FormatException("Invalid prescription format.");
            }

            var medicationId = prescriptionArgs[0].Trim();
            var medication = MedicationRepository.Instance.GetById(medicationId);
            var amount = int.Parse(prescriptionArgs[1].Trim());
            var dailyUsage = int.Parse(prescriptionArgs[2].Trim());
            var medicationTiming = Enum.Parse<MedicationTiming>(prescriptionArgs[3].Trim());
            var issuedDate = DateTime.ParseExact(prescriptionArgs[4], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var doctorId = prescriptionArgs[5].Trim();


            return new Prescription(medication, amount, dailyUsage, medicationTiming, doctorId) { IssuedDate = issuedDate };
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value is Prescription prescription)
            {
                return $"{prescription.Medication.Id};{prescription.Amount};{prescription.DailyUsage};{prescription.MedicationTiming};{prescription.IssuedDate.ToString("yyyy-MM-dd HH:mm:ss")}";
            }

            return string.Empty;
        }
    }
}
