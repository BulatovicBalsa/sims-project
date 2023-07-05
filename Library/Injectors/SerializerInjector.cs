using System;
using System.Collections.Generic;
using Library.Models;
using Library.Models.Books;
using Library.Models.Memberships;
using Library.Serialization;

namespace Library.Injectors;

public class SerializerInjector
{
    private static readonly Dictionary<Type, object> Implementations = new()
    {
        { typeof(ISerializer<Member>), new CsvSerializer<Member>() },
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