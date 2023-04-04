using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Patients;

namespace ZdravoCorp.Repositories
{
    internal class PatientRepository : IRepository<Patient>
    {
        private readonly string _csvFilePath;

        public PatientRepository(string csvFilePath) 
        {
            _csvFilePath = csvFilePath;
        }


        public void Add(Patient entity)
        {
            var records = GetAll().ToList();
            records.Add(entity);
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer,CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        public void Delete(Patient entity)
        {
            var records = GetAll().ToList();
            var existingEntity = records.FirstOrDefault(x => x.Id == entity.Id);
            if (existingEntity == null) 
            {
                throw new Exception($"Entity with id {entity.Id} not found");
            }
            records.Remove(existingEntity);
            using var writer = new StreamWriter( _csvFilePath); 
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);  
        }

        public IEnumerable<Patient> GetAll()
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Patient>().ToList();
        }

        public Patient? GetById(int id)
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Patient>().FirstOrDefault(x => x.Id == id);
        }

        public void Update(Patient entity)
        {
            var records = GetAll().ToList();
            var existingEntity = records.FirstOrDefault(x => x.Id == entity.Id);
            if(existingEntity == null)
            {
                throw new Exception($"Entity with id {entity.Id} not found");
            }
            var index = records.IndexOf(existingEntity);
            records[index] = entity;
            using var writer = new StreamWriter(_csvFilePath); 
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }
    }
}
