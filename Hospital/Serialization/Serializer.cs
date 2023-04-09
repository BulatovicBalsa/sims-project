using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Hospital.Serialization;

public class Serializer<T>
{
    public static List<T> FromCSV(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<T>().ToList();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return new List<T>();
        }
    }

    public static List<T> FromCSV(string filePath, ClassMap<T> mapper)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Context.RegisterClassMap(mapper);
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
        csvWriter.WriteRecords(records);
        csvWriter.Flush();
    }

    public static void ToCSV(List<T> records, string filePath, ClassMap<T> mapper)
    {
        using var writer = new StreamWriter(filePath);
        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csvWriter.Context.RegisterClassMap(mapper);
        csvWriter.WriteRecords(records);
        csvWriter.Flush();
    }
}