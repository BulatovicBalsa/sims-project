using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repositories.Manager;
using Hospital.Repositories.Patient;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Patient
{
    using Hospital.Models.Patient;
    [TestClass]
    public class PatientRepositoryTests
    {
        private const string TestFilePath = "../../../Data/patients.csv";
        [TestMethod]
        public void TestGetAll()
        {
            var testPatients = new List<Patient>
            {
                new("Vladimir", "Popov", "0123456789012", "vlada1234", "vlada1234", new MedicalRecord(175, 70, new List < string > { "penicillin", "sulfa", "aspirin" }, new List < string >() { "mental illness", "cold", })),
                new("Momir", "Milutinovic", "0123456789012", "momir1234", "momir1234", new MedicalRecord(176, 71, new List<string> { "penicillin", "sulfa", "aspirin" }, new List<string>() { "mental illness", "cold", })),
                new("Balsa", "Bulatovic", "0123456789012", "balsa1234", "balsa1234", new MedicalRecord(177, 72, new List < string > { "penicillin", "sulfa", "aspirin" }, new List < string >() { "mental illness", "cold", })),
                new("Teodor", "Vidakovic", "0123456789012", "teodor1234", "teodor1234", new MedicalRecord(178, 73, new List < string > { "penicillin", "sulfa", "aspirin" }, new List < string >() { "mental illness", "cold", })),
            };

            Serializer<Patient>.ToCSV(testPatients, TestFilePath, new PatientWriteMapper());

            var patientRepository = new PatientRepository();
            var loadedPatients = patientRepository.GetAll();

            Assert.IsNotNull(loadedPatients);
            Assert.AreEqual(4, loadedPatients.Count);
            Assert.AreEqual("Balsa", loadedPatients[2].FirstName);
            Assert.AreEqual(177, loadedPatients[2].MedicalRecord.Height);
            Assert.AreNotEqual(177, loadedPatients[0].MedicalRecord.Height);
            Assert.AreNotEqual("1111111111111", loadedPatients[3].Jmbg);
            Assert.IsTrue(loadedPatients[0].MedicalRecord.Allergies.Count == 3);
            Assert.IsTrue(loadedPatients[2].MedicalRecord.MedicalHistory.Count == 2);
            Assert.IsTrue(loadedPatients[3].MedicalRecord.MedicalHistory.Contains("mental illness"));
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
    }
}
