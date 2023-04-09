using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

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

    public static void ToCSV(List<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csvWriter.WriteRecords(records);
    }
}