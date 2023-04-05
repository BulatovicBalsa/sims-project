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
    internal class ExaminationChangesTrackerRepository : IRepository<PatientExaminationLog>
    {
        private readonly string _csvFilePath;

        public ExaminationChangesTrackerRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public void Add(PatientExaminationLog patientExaminationLog)
        {
            var records = GetAll().ToList();
            records.Add(patientExaminationLog);
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }
        public IEnumerable<PatientExaminationLog> GetAll()
        {
            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<PatientExaminationLog>().ToList();
        }


        public void Delete(PatientExaminationLog patientExaminationLog)
        {
            throw new NotImplementedException();
        }

        public PatientExaminationLog? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PatientExaminationLog patientExaminationLog)
        {
            throw new NotImplementedException();
        }
    }
}
