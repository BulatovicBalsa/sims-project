using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Patient;
using Hospital.Serialization;

namespace Hospital.Repositories.Patient
{
    public class ExaminationChangesTrackerRepository
    {
        private const string FilePath = "../../../Data/examinationChangesTrackerLogs.csv";

        public List<PatientExaminationLog> GetAll()
        {
            return Serializer<PatientExaminationLog>.FromCSV(FilePath);
        }

        public void Add(PatientExaminationLog log)
        {
            var allLogs = GetAll();
            allLogs.Add(log);
            Serializer<PatientExaminationLog>.ToCSV(allLogs,FilePath);
        }
    }
}
