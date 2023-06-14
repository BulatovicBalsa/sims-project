using System.Collections.Generic;
using CsvHelper.Configuration;

namespace Hospital.Serialization;

public interface ISerializer<T>
{
    List<T> Load(string filePath, ClassMap<T>? mapper = null);
    void Save(List<T> records, string filePath, ClassMap<T>? mapper = null);
}