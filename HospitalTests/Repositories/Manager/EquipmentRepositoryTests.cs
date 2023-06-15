using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentRepositoryTests

{
    [TestInitialize]
    public void SetUp()
    {
        EquipmentRepository.Instance.DeleteAll();
        var equipment = new List<Equipment>
        {
            new("1", "Chair", EquipmentType.Furniture),
            new("2", "Operating Table",
                EquipmentType.OperationEquipment),
            new("3", "Stethoscope", EquipmentType.ExaminationEquipment),
            new("4", "Wheelchair", EquipmentType.HallwayEquipment)
        };

        CsvSerializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");
    }

    [TestMethod]
    public void TestGetAll()
    {
        var equipmentRepository = EquipmentRepository.Instance;

        var loadedEquipment = equipmentRepository.GetAll();
        Assert.AreEqual(4, loadedEquipment.Count);
        Assert.AreEqual(EquipmentType.OperationEquipment, loadedEquipment[1].Type);
        Assert.AreEqual("Operating Table", loadedEquipment[1].Name);
    }

    [TestMethod]
    public void TestGetAllNonExistentFile()
    {
        if (File.Exists("../../../Data/equipment.csv")) File.Delete("../../../Data/equipment.csv");

        Assert.AreEqual(0, EquipmentRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestGetById()
    {
        var equipmentRepository = EquipmentRepository.Instance;

        Assert.AreEqual("Stethoscope", equipmentRepository.GetById("3").Name);
        Assert.AreEqual(EquipmentType.Furniture, equipmentRepository.GetById("1").Type);
        Assert.IsNull(equipmentRepository.GetById("0"));
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentRepository = EquipmentRepository.Instance;

        equipmentRepository.Update(new Equipment("1", "Table", EquipmentType.Furniture));

        Assert.AreEqual("Table", equipmentRepository.GetById("1").Name);
        Assert.AreEqual(EquipmentType.Furniture, equipmentRepository.GetById("1").Type);
    }

    [TestMethod]
    public void TestDelete()
    {
        var equipmentToDelete = new Equipment("2", "Operating table", EquipmentType.OperationEquipment);

        var equipmentRepository = EquipmentRepository.Instance;

        equipmentRepository.Delete(equipmentToDelete);

        Assert.AreEqual(3, equipmentRepository.GetAll().Count);
        Assert.IsNull(equipmentRepository.GetById("2"));
    }

    [TestMethod]
    public void TestAdd()
    {
        var newEquipment = new Equipment("5", "C-Arm", EquipmentType.OperationEquipment);

        var equipmentRepository = EquipmentRepository.Instance;

        equipmentRepository.Add(newEquipment);
        var loadedNewEquipment = equipmentRepository.GetById("5");

        Assert.AreEqual(5, equipmentRepository.GetAll().Count);
        Assert.AreEqual("C-Arm", loadedNewEquipment.Name);
        Assert.AreEqual(EquipmentType.OperationEquipment, loadedNewEquipment.Type);
    }

    [TestMethod]
    public void TestUpdateNonExistentEquipment()
    {
        var equipmentRepository = EquipmentRepository.Instance;

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Update(new Equipment("0", "Nonexistent",
            EquipmentType.Furniture)));
    }

    [TestMethod]
    public void TestDeleteNonExistentEquipment()
    {
        var equipmentRepository = EquipmentRepository.Instance;

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Delete(new Equipment("0", "Nonexistent",
            EquipmentType.Furniture)));
    }


    public void AddDynamicEquipmentToCsv()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", EquipmentType.Furniture),
            new("2", "Operating Table",
                EquipmentType.OperationEquipment),
            new("3", "Stethoscope", EquipmentType.ExaminationEquipment),
            new("4", "Wheelchair", EquipmentType.HallwayEquipment),
            new("5", "Pencil", EquipmentType.DynamicEquipment),
            new("6", "Paper", EquipmentType.DynamicEquipment),
            new("7", "Band-Aid", EquipmentType.DynamicEquipment),
            new("8", "Bandage", EquipmentType.DynamicEquipment),
            new("9", "Buckle", EquipmentType.DynamicEquipment)
        };

        CsvSerializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");
    }

    [TestMethod]
    public void TestGetNonDynamic()
    {
        AddDynamicEquipmentToCsv();
        var equipmentRepository = EquipmentRepository.Instance;
        Assert.AreEqual(4, equipmentRepository.GetNonDynamic().Count);
    }

    [TestMethod]
    public void TestGetDynamic()
    {
        AddDynamicEquipmentToCsv();
        var equipmentRepository = EquipmentRepository.Instance;
        Assert.AreEqual(5, equipmentRepository.GetDynamic().Count);
    }
}