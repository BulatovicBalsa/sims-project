using System;
using System.Collections.Generic;
using Hospital.Models.Doctor;
using Hospital.Models;
using Hospital.Models.Books;
using Hospital.Models.Memberships;
using Hospital.Serialization;

namespace Hospital.Injectors;

public class SerializerInjector
{
    private static readonly Dictionary<Type, object> Implementations = new()
    {
        { typeof(ISerializer<Doctor>), new CsvSerializer<Doctor>() },
        { typeof(ISerializer<Book>), new CsvSerializer<Book>() },
        { typeof(ISerializer<Loan>), new CsvSerializer<Loan>() },
        { typeof(ISerializer<Membership>), new CsvSerializer<Membership>() },
        { typeof(ISerializer<Author>), new CsvSerializer<Author>() },
        { typeof(ISerializer<Genre>), new CsvSerializer<Genre>() },
        // Add more implementations here
    };

    public static T CreateInstance<T>()
    {
        var type = typeof(T);

        if (Implementations.TryGetValue(type, out var implementation)) return (T)implementation;

        throw new ArgumentException($"No implementation found for type {type}");
    }
}