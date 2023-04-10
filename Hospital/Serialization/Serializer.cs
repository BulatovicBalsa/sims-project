using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Hospital.Serialization;

public class Serializer<T, M> where M : ClassMap<T>, new()
{
    public static List<T> FromCSV(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            if (!isDefaultType())
            {
                csvReader.Context.RegisterClassMap<M>();
            }
            return csvReader.GetRecords<T>().ToList();

        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return new List<T>();
        }
    }

    public static void ToCSV(List<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        if (!isDefaultType())
        {
            csvWriter.Context.RegisterClassMap<M>();
        }
        csvWriter.WriteRecords(records);
    }

    private static bool isDefaultType()
    {
        return typeof(M).Equals(typeof(DefaultClassMap<T>));
    }
}

public class Serializer<T>
{
    public static List<T> FromCSV(string filePath)
    {
        return Serializer<T, DefaultClassMap<T>>.FromCSV(filePath);
    }

    public static void ToCSV(List<T> records, string filePath)
    {
        Serializer<T, DefaultClassMap<T>>.ToCSV(records, filePath);
    }

}
