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

        private static ExaminationChangesTrackerRepository? _instance;
        public static ExaminationChangesTrackerRepository Instance => _instance ??= new ExaminationChangesTrackerRepository();
        private ExaminationChangesTrackerRepository() { }

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

        public static void DeleteAll()
        {
            Serializer<PatientExaminationLog>.ToCSV(new List<PatientExaminationLog>(), FilePath);
        }

    }
}
