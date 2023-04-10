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
        private PatientRepository _patientRepository;
        private Patient _patient;
        [TestInitialize]
        public void TestInitialize()
        {
            PatientRepository.DeleteAll();
            _patientRepository = CreateTestPatientRepository();
            _patient = CreateTestPatient();
            _patientRepository.Add(_patient);
        }
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
            var addedPatient = _patientRepository.GetById(_patient.Id);
            Assert.IsNotNull(addedPatient);

            PatientRepository.DeleteAll();
        }

        [TestMethod]
        public void TestGetById()
        {
            var foundPatient = _patientRepository.GetById(_patient.Id);

            Assert.IsNotNull(foundPatient);
            Assert.AreEqual(_patient.Id,foundPatient.Id);

            string nonExistingPatientId = "non-existing-id";

            foundPatient = _patientRepository.GetById(nonExistingPatientId);

            Assert.IsNull(foundPatient);

            PatientRepository.DeleteAll();
        }

        [TestMethod]
        public void TestUpdate()
        {
            _patient.FirstName = "UpdatedFirstName";

            _patientRepository.Update(_patient);

            var updatedPatient = _patientRepository.GetById(_patient.Id);
            Assert.IsNotNull(updatedPatient);
            Assert.AreEqual("UpdatedFirstName",updatedPatient.FirstName);

            PatientRepository.DeleteAll();
        }

        [TestMethod]
        public void TestDelete()
        {
            _patientRepository.Delete(_patient);

            var deletedPatient = _patientRepository.GetById(_patient.Id);
            Assert.IsNull(deletedPatient);

            PatientRepository.DeleteAll();
        }
        
    }
}
