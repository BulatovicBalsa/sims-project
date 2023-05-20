﻿using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using Hospital.Repositories.Patient;

namespace Hospital.Serialization.Mappers.Patient;

using CsvHelper.TypeConversion;
using CsvHelper;
using Models.Patient;
using Repositories.Doctor;

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
        Map(patient => patient.MedicalRecord.Prescriptions).Index(12).TypeConverter<PrescriptionTypeConverter>();
        Map(patient => patient.NotificationTime).Index(13);
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
            referrals.AddRange(from item in referralStringList select item.Split(";") into referralArgs 
                let doctorId = referralArgs[1].Trim() 
                let doctor = string.IsNullOrEmpty(doctorId) ? null : DoctorRepository.Instance.GetById(doctorId) 
                let specialization = referralArgs[0].Trim() select new Referral(specialization, doctor));

            return referrals;
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
                select item.Split(";") into prescriptionArgs
                let medicationId = prescriptionArgs[0].Trim()
                let medication = MedicationRepository.Instance.GetById(medicationId)
                let amount = Convert.ToInt32(prescriptionArgs[1])
                let dailyUsage = Convert.ToInt32(prescriptionArgs[2])
                let medicationTiming = (MedicationTiming)Enum.Parse(typeof(MedicationTiming), prescriptionArgs[3])
                select new Prescription(medication, amount, dailyUsage, medicationTiming));

            return prescriptions;
        }
    }
}