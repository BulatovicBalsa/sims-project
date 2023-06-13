using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using Hospital.Repositories.Patient;

namespace Hospital.Serialization.Mappers.Patient;

using CsvHelper.TypeConversion;
using CsvHelper;
using Models.Patient;
using Repositories.Doctor;
using System.Globalization;

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
        Map(patient => patient.MedicalRecord.Allergies.Conditions).Index(8)
            .Convert(row => SplitColumnValues(row.Row.GetField<string>("Allergies")));
        Map(patient => patient.MedicalRecord.MedicalHistory.Conditions).Index(9)
            .Convert(row => SplitColumnValues(row.Row.GetField<string>("MedicalHistory")));
        Map(patient => patient.IsBlocked).Index(10);
        Map(patient => patient.Referrals).Index(11).TypeConverter<ReferralTypeConverter>();
        Map(patient => patient.HospitalTreatmentReferrals).Index(12)
            .TypeConverter<HospitalTreatmentReferralTypeConverter>();
        Map(patient => patient.MedicalRecord.Prescriptions).Index(13).TypeConverter<PrescriptionTypeConverter>();
        Map(patient => patient.NotificationTime).Index(14);
    }

    private static List<string> SplitColumnValues(string? columnValue)
    {
        return columnValue?.Split("|").ToList() ?? new List<string>();
    }

    public class ReferralTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var referralStringList = SplitColumnValues(inputText);
            List<Referral> referrals = new();
            if (string.IsNullOrEmpty(referralStringList[0])) return referrals;
            referrals.AddRange(from item in referralStringList
                select item.Split(";")
                into referralArgs
                let doctorId = referralArgs.Length > 1 ? referralArgs[1].Trim() : null
                let doctor = string.IsNullOrEmpty(doctorId) ? null : DoctorRepository.Instance.GetById(doctorId)
                let specialization = referralArgs[0].Trim()
                select new Referral(specialization, doctor));

            return referrals;
        }
    }

    public class HospitalTreatmentReferralTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var referralStringList = SplitColumnValues(inputText);
            List<HospitalTreatmentReferral> referrals = new();
            if (string.IsNullOrEmpty(referralStringList[0])) return referrals;
            referrals.AddRange(from item in referralStringList
                select item.Split(";")
                into referralArgs
                let id = referralArgs[0]
                let duration = Convert.ToInt32(referralArgs[1].Trim())

                let prescriptions = referralArgs[2].Split("#").ToList()
                    .Select(PrescriptionFromString).Where(prescription => prescription != null).ToList()
                let additionalTests = referralArgs[3].Trim().Split("#")
                    .Where(additionalTest => !string.IsNullOrEmpty(additionalTest)).ToList()

                let admission = (string.IsNullOrEmpty(referralArgs[4])) ? (DateTime?)null : DateTime.ParseExact(referralArgs[4], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                let release = (string.IsNullOrEmpty(referralArgs[5])) ? (DateTime?)null : DateTime.ParseExact(referralArgs[5], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                let roomId = (string.IsNullOrEmpty(referralArgs[6])) ? null : referralArgs[6]

                select new HospitalTreatmentReferral(prescriptions, duration, additionalTests, admission, release, roomId){Id = id});

            return referrals;
        }

        private Prescription PrescriptionFromString(string prescriptionAsString)
        {
            if (string.IsNullOrEmpty(prescriptionAsString.Trim())) return null;
            var prescriptionFields = prescriptionAsString.Split("$");
            var medicationId = prescriptionFields[0].Trim();
            var medication = MedicationRepository.Instance.GetById(medicationId);
            var amount = Convert.ToInt32(prescriptionFields[1]);
            var dailyUsage = Convert.ToInt32(prescriptionFields[2]);
            var medicationTiming = (MedicationTiming)Enum.Parse(typeof(MedicationTiming), prescriptionFields[3]);
            var doctorId = prescriptionFields[4].Trim();
            return new Prescription(medication!, amount, dailyUsage, medicationTiming, doctorId);
        }
    }

    public class PrescriptionTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var prescriptionStringList = SplitColumnValues(inputText);
            List<Prescription> prescriptions = new();
            if (string.IsNullOrEmpty(prescriptionStringList[0])) return prescriptions;
            prescriptions.AddRange(from item in prescriptionStringList
                select item.Split(";")
                into prescriptionArgs
                let medicationId = prescriptionArgs[0].Trim()
                let medication = MedicationRepository.Instance.GetById(medicationId)
                let amount = Convert.ToInt32(prescriptionArgs[1])
                let dailyUsage = Convert.ToInt32(prescriptionArgs[2])
                let medicationTiming = (MedicationTiming)Enum.Parse(typeof(MedicationTiming), prescriptionArgs[3])
                let issuedDate = DateTime.ParseExact(prescriptionArgs[4], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                let lastUsed = (string.IsNullOrEmpty(prescriptionArgs[5])) ? (DateTime?)null : DateTime.ParseExact(prescriptionArgs[5], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                let doctorId = prescriptionArgs[6].Trim()
                select new Prescription(medication, amount, dailyUsage, medicationTiming, doctorId) { IssuedDate = issuedDate, LastUsed = lastUsed});

            return prescriptions;
        }
    }
}