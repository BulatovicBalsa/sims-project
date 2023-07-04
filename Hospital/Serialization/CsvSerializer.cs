using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using CsvHelper.Configuration;

namespace Hospital.Serialization;

public class CsvSerializer<T> : ISerializer<T>
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
        const int numberOfRetries = 3;
        const int delay = 500;
        StreamWriter writer = null;

        for (int i = 1; i <= numberOfRetries; i++)
        {
            try
            {
                writer = new StreamWriter(filePath);
                break;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e);
                Directory.CreateDirectory(DirectoryPath);
                writer = new StreamWriter(filePath);
                break;
            }
            catch (IOException) when (i < numberOfRetries)
            {
                Thread.Sleep(delay);
            }

        }

        if (writer == null) throw new IOException();

        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        if (mapper != null)
        {
            csvWriter.Context.RegisterClassMap(mapper);
        }
        csvWriter.WriteRecords(records.ToList());

        csvWriter.Flush();
    }

    public List<T> Load(string filePath, ClassMap<T>? mapper = null)
    {
        return FromCSV(filePath, mapper);
    }

    public void Save(List<T> records, string filePath, ClassMap<T>? mapper = null)
    {
        ToCSV(records, filePath, mapper);
    }
}

