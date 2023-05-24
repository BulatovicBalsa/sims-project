using Hospital.Serialization;
using Hospital.Models.Patient;
using Hospital.Serialization.Mappers;
using Hospital.Serialization.Mappers.Patient;

namespace HospitalTests.Repositories.Patient;

using Hospital.Repositories.Patient;

[TestClass]
public class MedicationRepositoryTests
{
    private const string TestFilePath = "../../../Data/medications.csv";

    [TestInitialize]
    public void TestInitialize()
    {
        var testMedications = new List<Medication>
        {
            new("Ibuprofen", new List<string> { "brufen" }),
            new("Acetaminophen", new List<string> { "Aspirin" }),
            new("Amoxicillin", new List<string> { "Penicillin", "Codeine", "Sulfonamide" }),
            new("Penicillin", new List<string> { "Penicillin G", "Amoxicillin" }),
            new("Sulfamethoxazole", new List<string> { "Sulfonamides" }),
            new("Metronidazole", new List<string> { "Nitroimidazoles" }),
            new("Cephalexin", new List<string> { "Cephalosporins" }),
            new("Diphenhydramine", new List<string> { "Antihistamines" })
        };


        Serializer<Medication>.ToCSV(testMedications, TestFilePath, new MedicationWriteMapper());
    }

    [TestMethod]
    public void TestGetAll()
    {
        var medicationRepository = MedicationRepository.Instance;
        var loadedMedications = medicationRepository.GetAll();

        Assert.IsNotNull(loadedMedications);
        Assert.AreEqual(8, loadedMedications.Count);
        Assert.AreEqual("Penicillin", loadedMedications[3].Name);
        Assert.AreEqual(3, loadedMedications[2].Allergens.Count);
        Assert.AreEqual("Antihistamines", loadedMedications[7].Allergens[0]);
    }

    [TestMethod]
    public void TestNonExistentFile()
    {
        if (File.Exists(TestFilePath)) File.Delete(TestFilePath);

        Assert.AreEqual(0, MedicationRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestGetById()
    {
        var medicationRepository = MedicationRepository.Instance;
        var loadedMedications = medicationRepository.GetAll();

        var testMedication = loadedMedications[0];

        Assert.AreEqual(testMedication.Name, medicationRepository.GetById(testMedication.Id)?.Name);
        Assert.IsNull(medicationRepository.GetById("0"));
        Assert.AreEqual(testMedication.Name, medicationRepository.GetById(testMedication.Id)?.Name);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var medicationRepository = MedicationRepository.Instance;
        var loadedMedications = medicationRepository.GetAll();

        var testMedication = loadedMedications[1];
        testMedication.Name = "TestName";
        testMedication.Allergens = new List<string>();
        medicationRepository.Update(testMedication);

        Assert.AreEqual("TestName", medicationRepository.GetById(testMedication.Id)?.Name);
        Assert.AreEqual(0, medicationRepository.GetById(testMedication.Id)?.Allergens.Count);
    }

    [TestMethod]
    public void TestDelete()
    {
        var medicationRepository = MedicationRepository.Instance;
        var loadedMedications = medicationRepository.GetAll();

        var testMedication = loadedMedications[0];

        medicationRepository.Delete(testMedication);

        Assert.AreEqual(7, medicationRepository.GetAll().Count);
        Assert.IsNull(medicationRepository.GetById(testMedication.Id));
    }

    [TestMethod]
    public void TestAdd()
    {
        var newMedication = new Medication("Ibuprofen", new List<string>());
        var medicationRepository = MedicationRepository.Instance;

        medicationRepository.Add(newMedication);
        var loadedMedications = medicationRepository.GetAll();

        var testMedication = loadedMedications[4];

        Assert.AreEqual(9, medicationRepository.GetAll().Count);
        Assert.AreEqual(testMedication, medicationRepository.GetById(testMedication.Id));
    }

    [TestMethod]
    public void TestUpdateNonExistent()
    {
        var medicationRepository = MedicationRepository.Instance;
        var newMedication = new Medication("TestName", new List<string> { "TestAllergen" });

        Assert.ThrowsException<KeyNotFoundException>(() => medicationRepository.Update(newMedication));
    }

    [TestMethod]
    public void TestDeleteNonExistent()
    {
        var medicationRepository = MedicationRepository.Instance;
        var newMedication = new Medication("TestName", new List<string> { "TestAllergen" });

        Assert.ThrowsException<KeyNotFoundException>(() => medicationRepository.Delete(newMedication));
    }
}