using System.Globalization;
using CsvHelper;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentRepositoryTests

{
    [TestInitialize]
    public void SetUp()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        
        Serializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");

    } 

    [TestMethod]
    public void TestGetAll()
    {

        var equipmentRepository = new EquipmentRepository();

        var loadedEquipment = equipmentRepository.GetAll();
        Assert.AreEqual(4, loadedEquipment.Count);
        Assert.AreEqual(Equipment.EquipmentType.OPERATION_EQUIPMENT, loadedEquipment[1].Type);
        Assert.AreEqual("Operating Table", loadedEquipment[1].Name);
    }

    [TestMethod]
    public void TestGetAllNonExistentFile()
    {
        if (File.Exists("../../../Data/equipment.csv")) File.Delete("../../../Data/equipment.csv");

        Assert.AreEqual(0, new EquipmentRepository().GetAll().Count);
    }

    [TestMethod]
    public void TestGetById()
    { 
        var equipmentRepository = new EquipmentRepository();

        Assert.AreEqual("Stethoscope", equipmentRepository.GetById("3").Name);
        Assert.AreEqual(Equipment.EquipmentType.FURNITURE, equipmentRepository.GetById("1").Type);
        Assert.IsNull(equipmentRepository.GetById("0"));
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Update(new Equipment("1", "Table", Equipment.EquipmentType.FURNITURE));

        Assert.AreEqual("Table", equipmentRepository.GetById("1").Name);
        Assert.AreEqual(Equipment.EquipmentType.FURNITURE, equipmentRepository.GetById("1").Type);
    }

    [TestMethod]
    public void TestDelete()
    {
        var equipmentToDelete = new Equipment("2", "Operating table", Equipment.EquipmentType.OPERATION_EQUIPMENT);
        
        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Delete(equipmentToDelete);

        Assert.AreEqual(3, equipmentRepository.GetAll().Count);
        Assert.IsNull(equipmentRepository.GetById("2"));
    }

    [TestMethod]
    public void TestAdd()
    {
        var newEquipment = new Equipment("5", "C-Arm", Equipment.EquipmentType.OPERATION_EQUIPMENT);

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Add(newEquipment);
        var loadedNewEquipment = equipmentRepository.GetById("5");

        Assert.AreEqual(5, equipmentRepository.GetAll().Count);
        Assert.AreEqual("C-Arm", loadedNewEquipment.Name);
        Assert.AreEqual(Equipment.EquipmentType.OPERATION_EQUIPMENT, loadedNewEquipment.Type);
    }

    [TestMethod]
    public void TestUpdateNonExistentEquipment()
    {
        var equipmentRepository = new EquipmentRepository();

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Update(new Equipment("0", "Nonexistent",
            Equipment.EquipmentType.FURNITURE)));
    }

    [TestMethod]
    public void TestDeleteNonExistentEquipment()
    {
        

        var equipmentRepository = new EquipmentRepository();

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Delete(new Equipment("0", "Nonexistent",
            Equipment.EquipmentType.FURNITURE)));
    }
}