using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repositories.Nurse;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Nurse
{
    using Hospital.Models.Nurse;
    using Hospital.Models.Patient;

    [TestClass]
    public class NurseRepositoryTests
    {
        private const string TestFilePath = "../../../Data/nurses.csv";

        [TestInitialize]
        public void TestInitialize()
        {
            var testNurses = new List<Nurse>
            {
                new("Vladimir", "Popov", "0123456789012", "vlada1234", "vlada1234"),
                new("Momir", "Milutinovic", "0123456789012", "momir1234", "momir1234"),
                new("Balsa", "Bulatovic", "0123456789012", "balsa1234", "balsa1234"),
                new("Teodor", "Vidakovic", "0123456789012", "teodor1234", "teodor1234"),
            };

            Serializer<Nurse>.ToCSV(testNurses, TestFilePath);
        }

        [TestMethod]
        public void TestGetAll()
        {
            var nurseRepository = new NurseRepository();
            var loadedNurses = nurseRepository.GetAll();

            Assert.IsNotNull(loadedNurses);
            Assert.AreEqual(4, loadedNurses.Count);
            Assert.AreEqual("Teodor", loadedNurses[3].FirstName);
            Assert.AreNotEqual("11", loadedNurses[0].Jmbg);
            Assert.AreEqual("Milutinovic", loadedNurses[1].LastName);
        }
    }
}
