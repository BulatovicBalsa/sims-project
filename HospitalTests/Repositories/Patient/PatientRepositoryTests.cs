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
    public class PatientRepositoryTests
    {
        private PatientRepository CreateTestPatientRepository()
        {
            var patientRepository = new PatientRepository();
            var testPatient1 = new Patient("Alice", "Smith", "1234567890124", "alicesmith", "password1", new MedicalRecord(80,180));
            var testPatient2 = new Patient("Bob", "Johnson", "1234567890125", "bobjohnson", "password2", new MedicalRecord(80, 180));
            var testPatient3 = new Patient("Charlie", "Williams", "1234567890126", "charliewilliams", "password3", new MedicalRecord(80, 180));


            patientRepository.Add(testPatient1);
            patientRepository.Add(testPatient2);
            patientRepository.Add(testPatient3);

            return patientRepository;
        }

        private Patient CreateTestPatient()
        {
            return new Patient("John", "Doe", "1234567890123", "johndoe", "password", new MedicalRecord(80, 180));

        }

        [TestMethod]
        public void TestAdd()
        {
            var patientRepository = CreateTestPatientRepository();
            var patient = CreateTestPatient();

            patientRepository.Add(patient);

            var addedPatient = patientRepository.GetById(patient.Id);
            Assert.IsNotNull(addedPatient);

            PatientRepository.DeleteAll();
        }

        [TestMethod]
        public void TestGetById()
        {
            var patientRepository = CreateTestPatientRepository();
            var patient = CreateTestPatient();
            patientRepository.Add(patient);

            var foundPatient = patientRepository.GetById(patient.Id);

            Assert.IsNotNull(foundPatient);
            Assert.AreEqual(patient.Id,foundPatient.Id);



            string nonExistingPatientId = "non-existing-id";

            foundPatient = patientRepository.GetById(nonExistingPatientId);

            Assert.IsNull(foundPatient);

            PatientRepository.DeleteAll();
        }

        [TestMethod]
        public void TestUpdate()
        {
            var patientRepository = CreateTestPatientRepository();
            var patient = CreateTestPatient();
            patientRepository.Add(patient);
            patient.FirstName = "UpdatedFirstName";

            patientRepository.Update(patient);

            var updatedPatient = patientRepository.GetById(patient.Id);
            Assert.IsNotNull(updatedPatient);
            Assert.AreEqual("UpdatedFirstName",updatedPatient.FirstName);

            PatientRepository.DeleteAll();
        }

        [TestMethod]
        public void TestDelete()
        {
            var patientRepository = CreateTestPatientRepository();
            var patient = CreateTestPatient();
            patientRepository.Add(patient);

            patientRepository.Delete(patient);

            var deletedPatient = patientRepository.GetById(patient.Id);
            Assert.IsNull(deletedPatient);

            PatientRepository.DeleteAll();
        }
        
    }
}
