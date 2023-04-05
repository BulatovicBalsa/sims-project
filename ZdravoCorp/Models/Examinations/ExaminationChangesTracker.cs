using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Patients;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Models.Examinations
{
    internal class ExaminationChangesTracker
    {
        private List<PatientExaminationLog> logs;

        private readonly ExaminationChangesTrackerRepository _examinationChangesTrackerRepository;    

        public void Add(PatientExaminationLog log)
        {
            _examinationChangesTrackerRepository.Add(log);
        }

        public int GetNumberOfChangeLogsForPatientInLast30Days(Patient patient)
        {
            IEnumerable<PatientExaminationLog > logs = _examinationChangesTrackerRepository.GetAll();
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);
            return logs.Count(log => log.Patient == patient && log.Timestamp>thirtyDaysAgo && !log.IsCreationLog);
        }

        public int GetNumberOfCreationLogsForPatientInLast30Days(Patient patient)
        {
            IEnumerable<PatientExaminationLog> logs = _examinationChangesTrackerRepository.GetAll();
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);
            return logs.Count(log => log.Patient == patient && log.Timestamp > thirtyDaysAgo && log.IsCreationLog);
        }
    }
}
