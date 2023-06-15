using Hospital.Exceptions;
using Hospital.Serialization;
using Hospital.Workers.Repositories;

namespace HospitalTests.Repositories.Nurse;

[TestClass]
public class NurseRepositoryTests
{
    private const string TestFilePath = "../../../Data/nurses.csv";

    [TestInitialize]
    public void TestInitialize()
    {
        var testNurses = new List<Hospital.Workers.Models.Nurse>
        {
            new("Vladimir", "Popov", "0123456789012", "vlada1234", "vlada1234"),
            new("Momir", "Milutinovic", "0123456789012", "momir1234", "momir1234"),
            new("Balsa", "Bulatovic", "0123456789012", "balsa1234", "balsa1234"),
            new("Teodor", "Vidakovic", "0123456789012", "teodor1234", "teodor1234")
        };

        CsvSerializer<Hospital.Workers.Models.Nurse>.ToCSV(testNurses, TestFilePath);
    }

    [TestMethod]
    public void TestGetAll()
    {
        var nurseRepository = NurseRepository.Instance;
        var loadedNurses = nurseRepository.GetAll();

        Assert.IsNotNull(loadedNurses);
        Assert.AreEqual(4, loadedNurses.Count);
        Assert.AreEqual("Teodor", loadedNurses[3].FirstName);
        Assert.AreNotEqual("11", loadedNurses[0].Jmbg);
        Assert.AreEqual("Milutinovic", loadedNurses[1].LastName);
    }

    [TestMethod]
    public void TestNonExistentFile()
    {
        if (File.Exists(TestFilePath)) File.Delete(TestFilePath);

        Assert.AreEqual(0, NurseRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestGetById()
    {
        var nurseRepository = NurseRepository.Instance;
        var loadedNurses = nurseRepository.GetAll();

        var testNurse = loadedNurses[0];

        Assert.AreEqual(testNurse.FirstName, nurseRepository.GetById(testNurse.Id).FirstName);
        Assert.IsNull(nurseRepository.GetById("0"));
        Assert.AreEqual(testNurse.LastName, nurseRepository.GetById(testNurse.Id).LastName);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var nurseRepository = NurseRepository.Instance;
        var loadedNurses = nurseRepository.GetAll();

        var testNurse = loadedNurses[1];
        testNurse.FirstName = "TestFirstName";
        testNurse.LastName = "TestLastName";
        testNurse.Profile.Username = "TestUsername";
        testNurse.Profile.Password = "TestPassword";

        nurseRepository.Update(testNurse);

        Assert.AreEqual("TestFirstName", nurseRepository.GetById(testNurse.Id)?.FirstName);
        Assert.AreEqual("TestLastName", nurseRepository.GetById(testNurse.Id)?.LastName);
        Assert.AreEqual("TestUsername", nurseRepository.GetById(testNurse.Id)?.Profile.Username);
        Assert.AreEqual("TestPassword", nurseRepository.GetById(testNurse.Id)?.Profile.Password);
    }

    [TestMethod]
    public void TestDelete()
    {
        var nurseRepository = NurseRepository.Instance;
        var loadedNurses = nurseRepository.GetAll();

        var testNurse = loadedNurses[1];

        nurseRepository.Delete(testNurse);

        Assert.AreEqual(3, nurseRepository.GetAll().Count);
        Assert.IsNull(nurseRepository.GetById(testNurse.Id));
    }

    [TestMethod]
    public void TestAdd()
    {
        var newNurse = new Hospital.Workers.Models.Nurse("TestFirstName", "TestLastName", "1234567890123", "testUsername",
            "testPassword");
        var nurseRepository = NurseRepository.Instance;

        nurseRepository.Add(newNurse);
        var loadedNurses = nurseRepository.GetAll();

        var testNurse = loadedNurses[4];

        Assert.AreEqual(5, nurseRepository.GetAll().Count);
        Assert.AreEqual(testNurse, nurseRepository.GetById(testNurse.Id));
    }

    [TestMethod]
    public void TestUpdateNonExistent()
    {
        var nurseRepository = NurseRepository.Instance;
        var newNurse = new Hospital.Workers.Models.Nurse("TestFirstName", "TestLastName", "1234567890123", "testUsername",
            "testPassword");

        Assert.ThrowsException<ObjectNotFoundException>(() => nurseRepository.Update(newNurse));
    }

    [TestMethod]
    public void TestDeleteNonExistent()
    {
        var nurseRepository = NurseRepository.Instance;
        var newNurse = new Hospital.Workers.Models.Nurse("TestFirstName", "TestLastName", "1234567890123", "testUsername",
            "testPassword");

        Assert.ThrowsException<ObjectNotFoundException>(() => nurseRepository.Delete(newNurse));
    }
}