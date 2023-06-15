﻿using System;
using System.Collections.Generic;
using Hospital.Core.Messaging.Models;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.Pharmacy.Models;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.TimeOffRequests.Models;
using Hospital.Core.Workers.Models;
using Hospital.Serialization;

namespace Hospital.Injectors;

public class SerializerInjector
{
    private static readonly Dictionary<Type, object> Implementations = new()
    {
        { typeof(ISerializer<Doctor>), new CsvSerializer<Doctor>() },
        { typeof(ISerializer<DoctorTimeOffRequest>), new CsvSerializer<DoctorTimeOffRequest>() },
        { typeof(ISerializer<EmailMessage>), new CsvSerializer<EmailMessage>() },
        { typeof(ISerializer<PatientExaminationLog>), new CsvSerializer<PatientExaminationLog>() },
        { typeof(ISerializer<Visit>), new CsvSerializer<Visit>() },
        { typeof(ISerializer<MedicationOrder>), new CsvSerializer<MedicationOrder>() },
        { typeof(ISerializer<TransferItem>), new JsonSerializer<TransferItem>() },
        { typeof(ISerializer<ComplexRenovation>), new JsonSerializer<ComplexRenovation>() }
        // Add more implementations here
    };

    public static T CreateInstance<T>()
    {
        var type = typeof(T);

        if (Implementations.TryGetValue(type, out var implementation)) return (T)implementation;

        throw new ArgumentException($"No implementation found for type {type}");
    }
}