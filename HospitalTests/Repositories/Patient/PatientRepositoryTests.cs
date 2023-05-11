using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Repositories.Patient;
using Hospital.Serialization;
using Hospital.Serialization.Mappers.Patient;

namespace HospitalTests.Repositories.Patient
{
    using Hospital.Models.Patient;
    [TestClass]
    public class PatientRepositoryTests
    {
        private const string TestFilePath = "../../../Data/patients.csv";

        [TestInitialize]
        public void TestInitialize()
        {
            var examinationFilePath = "../../../Data/examination.csv";
            if (File.Exists(examinationFilePath))
            {
                File.Delete(examinationFilePath);
            }

            var testPatients = new List<Patient>
            {
                new("Vladimir", "Popov", "0123456789012", "vlada1234", "vlada1234", new MedicalRecord(175, 70, new List < string > { "penicillin", "sulfa", "aspirin" }, new List < string >() { "mental illness", "cold", })),
                new("Momir", "Milutinovic", "0123456789012", "momir1234", "momir1234", new MedicalRecord(176, 71, new List<string> { "penicillin", "sulfa", "aspirin" }, new List<string>() { "mental illness", "cold", })),
                new("Balsa", "Bulatovic", "0123456789012", "balsa1234", "balsa1234", new MedicalRecord(177, 72, new List < string > { "penicillin", "sulfa", "aspirin" }, new List < string >() { "mental illness", "cold", })),
                new("Teodor", "Vidakovic", "0123456789012", "teodor1234", "teodor1234", new MedicalRecord(178, 73, new List < string > { "penicillin", "sulfa", "aspirin" }, new List < string >() { "mental illness", "cold", })),
            };

            Serializer<Patient>.ToCSV(testPatients, TestFilePath, new PatientWriteMapper());
        }

        [TestMethod]
        public void TestGetAll()
        {
            var patientRepository = new PatientRepository();
            var loadedPatients = patientRepository.GetAll();

            Assert.IsNotNull(loadedPatients);
            Assert.AreEqual(4, loadedPatients.Count);
            Assert.AreEqual("Balsa", loadedPatients[2].FirstName);
            Assert.AreEqual(177, loadedPatients[2].MedicalRecord.Height);
            Assert.AreNotEqual(177, loadedPatients[0].MedicalRecord.Height);
            Assert.AreNotEqual("1111111111111", loadedPatients[3].Jmbg);
            Assert.IsTrue(loadedPatients[0].MedicalRecord.Allergies.Conditions.Count == 3);
            Assert.IsTrue(loadedPatients[2].MedicalRecord.MedicalHistory.Conditions.Count == 2);
            Assert.IsTrue(loadedPatients[3].MedicalRecord.MedicalHistory.Conditions.Contains("mental illness"));
        }

        [TestMethod]
        public void TestNonExistentFile()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            Assert.AreEqual(0, new PatientRepository().GetAll().Count);
        }

        [TestMethod]
        public void TestGetById()
        {
            var patientRepository = new PatientRepository();
            var loadedPatients = patientRepository.GetAll();

            var testPatient = loadedPatients[0];
            Assert.AreEqual(testPatient.FirstName, patientRepository.GetById(testPatient.Id)?.FirstName);
            Assert.IsNull(patientRepository.GetById("0"));
            Assert.AreEqual(testPatient.MedicalRecord.MedicalHistory.Conditions.Count, patientRepository.GetById(testPatient.Id)?.MedicalRecord.MedicalHistory.Conditions.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var patientRepository = new PatientRepository();
            patientRepository.PatientAdded += _ => { };
            patientRepository.PatientUpdated += _ => { };

            var loadedPatients = patientRepository.GetAll();

            var testPatient = loadedPatients[1];
            testPatient.FirstName = "TestFirstName";
            testPatient.LastName = "TestLastName";
            testPatient.MedicalRecord.Allergies.Conditions = new List<string> { "testAllergy" };

            patientRepository.Update(testPatient);

            Assert.AreEqual("TestFirstName", patientRepository.GetById(testPatient.Id)?.FirstName);
            Assert.AreEqual("TestLastName", patientRepository.GetById(testPatient.Id)?.LastName);
            Assert.AreEqual(1, patientRepository.GetById(testPatient.Id)?.MedicalRecord.Allergies.Conditions.Count);
            Assert.AreEqual("testAllergy", patientRepository.GetById(testPatient.Id)?.MedicalRecord.Allergies.Conditions[0]);
        }

        private void PatientRepository_PatientUpdated(Patient obj)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestDelete()
        {
            var patientRepository = new PatientRepository();
            var loadedPatients = patientRepository.GetAll();

            var testPatient = loadedPatients[1];

            patientRepository.Delete(testPatient);

            Assert.AreEqual(3, patientRepository.GetAll().Count);
            Assert.IsNull(patientRepository.GetById(testPatient.Id));
        }

        [TestMethod]
        public void TestAdd()
        {
            var newPatient = new Patient("TestFirstName", "TestLastName", "1234567890123", "testUsername",
                "testPassword", new MedicalRecord(179, 80));
            var patientRepository = new PatientRepository();
            patientRepository.PatientAdded += _ => { };
            patientRepository.PatientUpdated += _ => { };

            patientRepository.Add(newPatient);
            var loadedPatients = patientRepository.GetAll();

            var testPatient = loadedPatients[4];

            Assert.AreEqual(5, patientRepository.GetAll().Count);
            Assert.AreEqual(testPatient, patientRepository.GetById(testPatient.Id));
        }

        [TestMethod]
        public void TestUpdateNonExistent()
        {
            var patientRepository = new PatientRepository();
            var newPatient = new Patient("TestFirstName", "TestLastName", "1234567890123", "testUsername",
                "testPassword", new MedicalRecord(179, 80));

            Assert.ThrowsException<KeyNotFoundException>(() => patientRepository.Update(newPatient));
        }

        [TestMethod]
        public void TestDeleteNonExistent()
        {
            var patientRepository = new PatientRepository();
            var newPatient = new Patient("TestFirstName", "TestLastName", "1234567890123", "testUsername",
                "testPassword", new MedicalRecord(179, 80));

            Assert.ThrowsException<KeyNotFoundException>(() => patientRepository.Delete(newPatient));
        }
    }
}