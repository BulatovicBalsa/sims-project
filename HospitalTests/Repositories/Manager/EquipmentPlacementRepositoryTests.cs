using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentPlacementRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        InventoryItemRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();
        var equipment = new List<Equipment>
        {
            new("1", "Chair", EquipmentType.Furniture),
            new("2", "Operating table",
                EquipmentType.OperationEquipment)
        };

        CsvSerializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");

        var equipmentPlacements = new List<InventoryItem>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        CsvSerializer<InventoryItem>.ToCSV(equipmentPlacements, "../../../Data/equipmentItems.csv");
    }


    [TestMethod]
    public void TestGetAll()
    {
        Assert.AreEqual(2, InventoryItemRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestAdd()
    {
        var equipmentItemRepository = InventoryItemRepository.Instance;
        equipmentItemRepository.Add(new InventoryItem("3", "3", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll().Count);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentItemRepository = InventoryItemRepository.Instance;
        equipmentItemRepository.Update(new InventoryItem("2", "2", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll()[1].Amount);
    }

    [TestMethod]
    public void TestDelete()
    {
        InventoryItemRepository.Instance.DeleteAll();
        var equipmentPlacements = new List<InventoryItem>
        {
            new("2", "1", 1),
            new("2", "2", 2)
        };
        CsvSerializer<InventoryItem>.ToCSV(equipmentPlacements, "../../../Data/equipmentItems.csv");


        var equipmentItemRepository = InventoryItemRepository.Instance;
        equipmentItemRepository.Delete(equipmentPlacements[1]);

        Assert.AreEqual(1, equipmentItemRepository.GetAll().Count);
        Assert.AreEqual(1, equipmentItemRepository.GetAll()[0].Amount);
        Assert.AreEqual("1", equipmentItemRepository.GetAll()[0].RoomId);
    }

    [TestMethod]
    public void TestGetAllJoinWithEquipment()
    {
        var loadedEquipmentPlacements = InventoryItemRepository.Instance.GetAll();

        Assert.AreEqual(2, loadedEquipmentPlacements.Count);
        Assert.IsNotNull(loadedEquipmentPlacements[0].Equipment);
        Assert.IsNotNull(loadedEquipmentPlacements[1].Equipment);
        Assert.AreEqual("Chair", loadedEquipmentPlacements[0].Equipment?.Name);
        Assert.AreEqual("Operating table", loadedEquipmentPlacements[1].Equipment?.Name);
    }
}