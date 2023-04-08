using CsvHelper;
using Hospital.Models.Manager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Serialization
{
    public class Serializer<T>
    {
        public static List<T> FromCSV(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<T>().ToList();
        }

        public static void ToCSV(List<T> records, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            using CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(records);
        }
    }
}
