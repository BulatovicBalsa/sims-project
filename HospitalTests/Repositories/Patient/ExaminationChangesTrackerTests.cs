using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace HospitalTests.Repositories.Patient
{
    using Hospital.Models.Patient;
    [TestClass]
    public class ExaminationChangesTrackerTests
    {
        private ExaminationChangesTracker _examinationChangesTracker;
        private ExaminationChangesTrackerRepository _examinationChangesTrackerRepository;
        private Patient _patient;

        [TestInitialize]
        public void TestInitialize()
        {
            _examinationChangesTrackerRepository = new ExaminationChangesTrackerRepository();
            _examinationChangesTracker = new ExaminationChangesTracker(_examinationChangesTrackerRepository);

            _examinationChangesTrackerRepository.DeleteAll();
            _patient = new Patient("Alice", "Smith", "1234567890124", "alicesmith", "password1", new MedicalRecord(80, 180));

        }

        [TestMethod]
        public void TestAdd()
        {
            var log = new PatientExaminationLog(_patient, false);

            _examinationChangesTracker.Add(log);

            var logs = _examinationChangesTrackerRepository.GetAll();
            Assert.IsTrue(logs.Contains(log));

            _examinationChangesTrackerRepository.DeleteAll();
        }

        [TestMethod]
        public void TestGetNumberOfChangeLogsForPatientInLast30Days()
        {
            var now = DateTime.Now;
            var recentLog = new PatientExaminationLog
            {
                Patient = _patient,
                Timestamp = now.AddDays(-10),
                IsCreationLog = false
            };
            var oldLog = new PatientExaminationLog
            {
                Patient = _patient,
                Timestamp = now.AddDays(-35),
                IsCreationLog = false
            };

            _examinationChangesTracker.Add(recentLog);
            _examinationChangesTracker.Add(oldLog);

            var count = _examinationChangesTracker.GetNumberOfChangeLogsForPatientInLast30Days(_patient);

            Assert.AreEqual(1, count);

            _examinationChangesTrackerRepository.DeleteAll();
        }

        [TestMethod]
        public void TestGetNumberOfCreationLogsForPatientInLast30Days()
        {
            var now = DateTime.Now;
            var recentLog = new PatientExaminationLog
            {
                Patient = _patient,
                Timestamp = now.AddDays(-10),
                IsCreationLog = true
            };
            var oldLog = new PatientExaminationLog
            {
                Patient = _patient,
                Timestamp = now.AddDays(-35),
                IsCreationLog = true
            };

            _examinationChangesTracker.Add(recentLog);
            _examinationChangesTracker.Add(oldLog);

            var count = _examinationChangesTracker.GetNumberOfCreationLogsForPatientInLast30Days(_patient);

            Assert.AreEqual(1, count);

            _examinationChangesTrackerRepository.DeleteAll();

        }


    }
}
