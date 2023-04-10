using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;

namespace HospitalTests.Repositories.Examination
{
    using Hospital.Models.Doctor;
    using Hospital.Models.Patient;
    using Hospital.Models.Examination;
    [TestClass]
    public class ExaminationRepositoryTests
    {
        private ExaminationChangesTrackerRepository _examinationChangesTrackerRepository;
        private ExaminationChangesTracker _examinationChangesTracker;
        private ExaminationRepository _examinationRepository;
        private Examination _examination;

        [TestInitialize]
        public void TestInitialize()
        {
            ExaminationRepository.DeleteAll();
            ExaminationChangesTrackerRepository.DeleteAll();

            CreateTestExaminationRepository();
            CreateTestExamination();
            _examinationRepository.Add(_examination, true);
        }
        private void CreateTestExaminationRepository()
        {
            _examinationChangesTrackerRepository = new ExaminationChangesTrackerRepository();
            _examinationChangesTracker = new ExaminationChangesTracker(_examinationChangesTrackerRepository);
            _examinationRepository = new ExaminationRepository(_examinationChangesTracker);

            var doctor1 = new Doctor("Dr. Emily", "Brown", "1234567890121", "dremilybrown", "docpassword1");
            var doctor2 = new Doctor("Dr. Jake", "Wilson", "1234567890122", "drjakewilson", "docpassword2");

            var patient1 = new Patient("Alice", "Smith", "1234567890124", "alicesmith", "password1", new MedicalRecord(80, 180));
            var patient2 = new Patient("Bob", "Johnson", "1234567890125", "bobjohnson", "password2", new MedicalRecord(80, 180));

            var examination1 = new Examination(doctor1, patient1, false, DateTime.Now.AddHours(30));
            var examination2 = new Examination(doctor1, patient2, false, DateTime.Now.AddHours(40));
            var examination3 = new Examination(doctor2, patient1, true, DateTime.Now.AddHours(50));

            _examinationRepository.Add(examination1,true);
            _examinationRepository.Add(examination2, true);
            _examinationRepository.Add(examination3, false);
        }

        private void CreateTestExamination()
        {
            var doctor = new Doctor("Dr. Linda", "Miller", "1234567890123", "drlindamiller", "docpassword3");
            var patient = new Patient("Charlie", "Williams", "1234567890126", "charliewilliams", "password3", new MedicalRecord(80, 180));

            _examination = new Examination(doctor, patient, false, DateTime.Now.AddHours(60));
        }

        [TestMethod]
        public void TestAdd()
        {
            var addedExamination = _examinationRepository.GetById(_examination.Id);
            Assert.IsNotNull(addedExamination);
        }

        [TestMethod]
        public void TestUpdate()
        {
            _examination.Start = _examination.Start.AddHours(5);
            _examination.IsOperation = true;
                
            _examinationRepository.Update(_examination, false);

            var updatedExamination = _examinationRepository.GetById(_examination.Id);

            Assert.IsNotNull(updatedExamination);
            Assert.AreEqual(_examination.Id, updatedExamination.Id);
            Assert.AreEqual(_examination.Doctor, updatedExamination.Doctor);
            Assert.AreEqual(_examination.Patient, updatedExamination.Patient);
            Assert.AreEqual(_examination.IsOperation, updatedExamination.IsOperation);

            
            const double tolerance = 1; // 1 second
            var secondsDifference = Math.Abs((_examination.Start - updatedExamination.Start).TotalSeconds);
            Assert.IsTrue(secondsDifference <= tolerance, "The Start times are not equal within the tolerance value.");
        }

        [TestMethod]
        public void TestDelete()
        {
            _examinationRepository.Delete(_examination, true);

            var deletedExaminaton = _examinationRepository.GetById(_examination.Id);
            Assert.IsNull(deletedExaminaton);
        }
    }
}
