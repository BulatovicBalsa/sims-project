using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Examinations;

namespace ZdravoCorp.Repositories
{
    internal class ExaminationChangesTrackerRepository : IRepository<ExaminationChangesTracker>
    {
        private readonly string _csvFilePath;

        public ExaminationChangesTrackerRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public void Add(ExaminationChangesTracker examinationChangesTracker)
        {
            var records = GetAll().ToList();
            records.Add(examinationChangesTracker);
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        public void Delete(ExaminationChangesTracker examinationChangesTracker)
        {
            var records = GetAll().ToList();
            //TO DO
        }

        public IEnumerable<ExaminationChangesTracker> GetAll()
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<ExaminationChangesTracker>().ToList();
        }

        public ExaminationChangesTracker? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ExaminationChangesTracker examinationChangesTracker)
        {
            throw new NotImplementedException();
        }
    }
}
