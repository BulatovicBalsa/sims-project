using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.Repositories
{
    public class ExaminationRepository: IRepository<Examination>
    {
        private readonly string _csvFilePath;

        public ExaminationRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public IEnumerable<Examination> GetAll()
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Examination>().ToList();
        }

        public Examination? GetById(int id)
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Examination>().FirstOrDefault(x => x.Id == id);
        }

        public void Add(Examination entity)
        {
            var records = GetAll().ToList();
            records.Add(entity);
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        public void Update(Examination entity)
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

        public void Delete(Examination entity)
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
