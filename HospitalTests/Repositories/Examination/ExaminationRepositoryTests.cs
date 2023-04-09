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
        private ExaminationRepository CreateTestExaminationRepository()
        {
            var examinationChangesTrackerRepository = new ExaminationChangesTrackerRepository();
            var examinationChangesTracker = new ExaminationChangesTracker(examinationChangesTrackerRepository);
            var examinationRepository = new ExaminationRepository(examinationChangesTracker);

            var doctor1 = new Doctor("Dr. Emily", "Brown", "1234567890121", "dremilybrown", "docpassword1");
            var doctor2 = new Doctor("Dr. Jake", "Wilson", "1234567890122", "drjakewilson", "docpassword2");

            var patient1 = new Patient("Alice", "Smith", "1234567890124", "alicesmith", "password1", new MedicalRecord(80, 180));
            var patient2 = new Patient("Bob", "Johnson", "1234567890125", "bobjohnson", "password2", new MedicalRecord(80, 180));

            var examination1 = new Examination(doctor1, patient1, false, DateTime.Now.AddHours(30));
            var examination2 = new Examination(doctor1, patient2, false, DateTime.Now.AddHours(40));
            var examination3 = new Examination(doctor2, patient1, true, DateTime.Now.AddHours(50));

            examinationRepository.Add(examination1,true);
            examinationRepository.Add(examination2, true);
            examinationRepository.Add(examination3, false);

            return examinationRepository;

        }

        private Examination CreateTestExamination()
        {
            var doctor = new Doctor("Dr. Linda", "Miller", "1234567890123", "drlindamiller", "docpassword3");
            var patient = new Patient("Charlie", "Williams", "1234567890126", "charliewilliams", "password3", new MedicalRecord(80, 180));

            return new Examination(doctor, patient, false, DateTime.Now.AddHours(30));
        }

        [TestMethod]
        public void TestAdd()
        {
            var examinationRepository = CreateTestExaminationRepository();
            var examination = CreateTestExamination();

            examinationRepository.Add(examination,true);

            var addedExamination = examinationRepository.GetById(examination.Id);
            Assert.IsNotNull(addedExamination);

            examinationRepository.DeleteAll();

        }

        [TestMethod]
        //need to fix this one
        public void TestUpdate()
        {
            var examinationChangesTracker = new ExaminationChangesTracker();
            var examinationRepository = new ExaminationRepository(examinationChangesTracker);

            var testExamination = CreateTestExamination();

            examinationRepository.Add(testExamination, false);

            testExamination.Start = testExamination.Start.AddHours(5);
            testExamination.IsOperation = true;
                
            examinationRepository.Update(testExamination, false);

            var updatedExamination = examinationRepository.GetById(testExamination.Id);

            Assert.IsNotNull(updatedExamination);
            Assert.AreEqual(testExamination.Id, updatedExamination.Id);
            Assert.AreEqual(testExamination.Doctor, updatedExamination.Doctor);
            Assert.AreEqual(testExamination.Patient, updatedExamination.Patient);
            Assert.AreEqual(testExamination.Start, updatedExamination.Start);
            Assert.AreEqual(testExamination.IsOperation, updatedExamination.IsOperation);

            examinationRepository.DeleteAll();
        }

        [TestMethod]
        public void TestDelete()
        {
            var examinationRepository = CreateTestExaminationRepository();
            var examination = CreateTestExamination();
            examinationRepository.Add(examination, true);

            examinationRepository.Delete(examination, true);

            var deletedExaminaton = examinationRepository.GetById(examination.Id);
            Assert.IsNull(deletedExaminaton);

            examinationRepository.DeleteAll();
        }
    }
}
