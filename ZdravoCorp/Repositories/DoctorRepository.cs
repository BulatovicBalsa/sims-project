using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Doctors;
using CsvHelper;

namespace ZdravoCorp.Repositories
{
    public class DoctorRepository : IRepository<Doctor>
    {
        private readonly string _csvFilePath;

        public DoctorRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public IEnumerable<Doctor> GetAll()
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Doctor>().ToList();
        }

        public Doctor? GetById(int id)
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Doctor>().FirstOrDefault(x => x.Id == id);
        }

        public void Add(Doctor entity)
        {
            var records = GetAll().ToList();
            entity.Id = records.Any() ? records.Max(x => x.Id) + 1 : 1;
            records.Add(entity);
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        public void Update(Doctor entity)
        {
            var records = GetAll().ToList();
            var existingEntity = records.FirstOrDefault(x => x.Id == entity.Id);
            if (existingEntity == null)
            {
                throw new Exception($"Entity with id {entity.Id} not found");
            }
            var index = records.IndexOf(existingEntity);
            records[index] = entity;
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        public void Delete(Doctor entity)
        {
            var records = GetAll().ToList();
            var existingEntity = records.FirstOrDefault(x => x.Id == entity.Id);
            if (existingEntity == null)
            {
                throw new Exception($"Entity with id {entity.Id} not found");
            }
            records.Remove(existingEntity);
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }
    }
}
