using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Exceptions;
using Hospital.Repositories;
using Hospital.Serialization;
using static Hospital.Repositories.LibrarianRepository;

namespace HospitalTests.Repositories.Librarian
{
    using static Hospital.Models.Librarian;

    [TestClass]
    public class LibrarianRepositoryTests
    {
        private const string TestFilePath = "../../../Data/librarians.csv";

        [TestInitialize]
        public void TestInitialize()
        {
            var testLibrarians = new List<Hospital.Models.Librarian>
            {
                new("Vladimir", "Popov", "0123456789012", "vlada1234", "vlada1234"),
                new("Momir", "Milutinovic", "0123456789012", "momir1234", "momir1234"),
                new("Balsa", "Bulatovic", "0123456789012", "balsa1234", "balsa1234"),
                new("Teodor", "Vidakovic", "0123456789012", "teodor1234", "teodor1234"),
            };

            CsvSerializer<Hospital.Models.Librarian>.ToCSV(testLibrarians, TestFilePath);
        }

        [TestMethod]
        public void TestGetAll()
        {
            var librarianRepository = LibrarianRepository.Instance;
            var loadedLibrarians = librarianRepository.GetAll();

            Assert.IsNotNull(loadedLibrarians);
            Assert.AreEqual(4, loadedLibrarians.Count);
            Assert.AreEqual("Teodor", loadedLibrarians[3].FirstName);
            Assert.AreNotEqual("11", loadedLibrarians[0].Jmbg);
            Assert.AreEqual("Milutinovic", loadedLibrarians[1].LastName);
        }

        [TestMethod]
        public void TestNonExistentFile()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            Assert.AreEqual(0, LibrarianRepository.Instance.GetAll().Count);
        }

        [TestMethod]
        public void TestGetById()
        {
            var librarianRepository = LibrarianRepository.Instance;
            var loadedLibrarians = librarianRepository.GetAll();

            var testLibrarian = loadedLibrarians[0];
            
            Assert.AreEqual(testLibrarian.FirstName, librarianRepository.GetById(testLibrarian.Id).FirstName);
            Assert.IsNull(librarianRepository.GetById("0"));
            Assert.AreEqual(testLibrarian.LastName, librarianRepository.GetById(testLibrarian.Id).LastName);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var librarianRepository = LibrarianRepository.Instance;
            var loadedLibrarians= librarianRepository.GetAll();

            var testLibrarian = loadedLibrarians[1];
            testLibrarian.FirstName = "TestFirstName";
            testLibrarian.LastName = "TestLastName";
            testLibrarian.Profile.Username = "TestUsername";
            testLibrarian.Profile.Password = "TestPassword";

            librarianRepository.Update(testLibrarian);

            Assert.AreEqual("TestFirstName", librarianRepository.GetById(testLibrarian.Id)?.FirstName);
            Assert.AreEqual("TestLastName", librarianRepository.GetById(testLibrarian.Id)?.LastName);
            Assert.AreEqual("TestUsername", librarianRepository.GetById(testLibrarian.Id)?.Profile.Username);
            Assert.AreEqual("TestPassword", librarianRepository.GetById(testLibrarian.Id)?.Profile.Password);
        }

        [TestMethod]
        public void TestDelete()
        {
            var librarianRepository = LibrarianRepository.Instance;
            var loadedLibrarians= librarianRepository.GetAll();

            var testLibrarian= loadedLibrarians[1];

            librarianRepository.Delete(testLibrarian);

            Assert.AreEqual(3, librarianRepository.GetAll().Count);
            Assert.IsNull(librarianRepository.GetById(testLibrarian.Id));
        }

        [TestMethod]
        public void TestAdd()
        {
            var newLibrarian = new Hospital.Models.Librarian("TestFirstName", "TestLastName", "1234567890123", "testUsername",
                "testPassword");
            var librarianRepository = LibrarianRepository.Instance;

            librarianRepository.Add(newLibrarian);
            var loadedLibrarians= librarianRepository.GetAll();

            var testLibrarian= loadedLibrarians[4];

            Assert.AreEqual(5, librarianRepository.GetAll().Count);
            Assert.AreEqual(testLibrarian, librarianRepository.GetById(testLibrarian.Id));
        }

        [TestMethod]
        public void TestUpdateNonExistent()
        {
            var librarianRepository = LibrarianRepository.Instance;
            var newLibrarian = new Hospital.Models.Librarian("TestFirstName", "TestLastName", "1234567890123", "testUsername",
                "testPassword");

            Assert.ThrowsException<ObjectNotFoundException>(() => librarianRepository.Update(newLibrarian));
        }

        [TestMethod]
        public void TestDeleteNonExistent()
        {
            var librarianRepository = LibrarianRepository.Instance;
            var newLibrarian = new Hospital.Models.Librarian("TestFirstName", "TestLastName", "1234567890123", "testUsername",
                "testPassword");

            Assert.ThrowsException<ObjectNotFoundException>(() => librarianRepository.Delete(newLibrarian));
        }
    }
}
