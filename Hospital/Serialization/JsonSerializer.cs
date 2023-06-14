﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Hospital.Models.Manager;
using Newtonsoft.Json;

namespace Hospital.Serialization
{
    public class JsonSerializer<T>: ISerializer<T>
    {
        public List<T> Load(string filePath, ClassMap<T>? mapper = null)
        {
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filePath)) ??
                                  throw new InvalidOperationException();
        }

        public void Save(List<T> records, string filePath, ClassMap<T>? mapper = null)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(records));
        }
    }
}