﻿using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using Hospital.Serialization;
using System.Collections.Generic;
using System;
using Hospital.Models.Patient;
using Hospital.Models.Requests;

namespace Hospital.Injectors;

public class SerializerInjector
{
    private static readonly Dictionary<Type, object> Implementations = new()
    {
        { typeof(ISerializer<Doctor>), new CsvSerializer<Doctor>() },
        { typeof(ISerializer<DoctorTimeOffRequest>), new CsvSerializer<DoctorTimeOffRequest>() },
        { typeof(ISerializer<Visit>), new CsvSerializer<Visit>() },
        { typeof(ISerializer<MedicationOrder>), new CsvSerializer<MedicationOrder>() }
        // Add more implementations here
    };

    public static T CreateInstance<T>()
    {
        var type = typeof(T);

        if (Implementations.TryGetValue(type, out var implementation))
        {
            return (T)implementation;
        }

        throw new ArgumentException($"No implementation found for type {type}");
    }
}