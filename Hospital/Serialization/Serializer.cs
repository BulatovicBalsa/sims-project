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
    private const string DirectoryPath = "../../../Data/";

    public static List<T> FromCSV(string filePath, ClassMap<T>? mapper = null)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            if (mapper != null)
            {
                csvReader.Context.RegisterClassMap(mapper);
            }

            return csvReader.GetRecords<T>().ToList();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return new List<T>();
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
            Directory.CreateDirectory(DirectoryPath);
            return new List<T>();
        }
    }

    public static void ToCSV(List<T> records, string filePath, ClassMap<T>? mapper = null)
    {
        StreamWriter writer;

        try
        {
            writer = new StreamWriter(filePath);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
            Directory.CreateDirectory(DirectoryPath);
            writer = new StreamWriter(filePath);
        }

        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        if (mapper != null)
        {
            csvWriter.Context.RegisterClassMap(mapper);
        }

        csvWriter.WriteRecords(records);

        csvWriter.Flush();
    }
}