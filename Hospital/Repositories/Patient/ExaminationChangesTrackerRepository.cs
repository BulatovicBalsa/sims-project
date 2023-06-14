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
        private ISerializer<PatientExaminationLog> _serializer;
        public ExaminationChangesTrackerRepository(ISerializer<PatientExaminationLog> serializer) 
        { 
            _serializer = serializer;
        }

        public List<PatientExaminationLog> GetAll()
        {
            return _serializer.Load(FilePath);
        }

        public void Add(PatientExaminationLog log)
        {
            var allLogs = GetAll();
            allLogs.Add(log);
            _serializer.Save(allLogs,FilePath);
        }

        public void DeleteAll()
        {
            _serializer.Save(new List<PatientExaminationLog>(), FilePath);
        }

    }
}
