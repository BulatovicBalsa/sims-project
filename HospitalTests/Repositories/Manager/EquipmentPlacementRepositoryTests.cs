using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;
using NuGet.Frameworks;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentPlacementRepositoryTests
{
    [TestMethod]
    public void TestGetAll()
    {
        var equipmentItems = new List<EquipmentPlacement>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        Assert.AreEqual(2, new EquipmentPlacementRepository().GetAll().Count);
    }

    [TestMethod]
    public void TestAdd()
    {
        var equipmentItems = new List<EquipmentPlacement>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipmentItemRepository = new EquipmentPlacementRepository();
        equipmentItemRepository.Add(new EquipmentPlacement("3", "3", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll().Count);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentItems = new List<EquipmentPlacement>
        {
            new("2", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipmentItemRepository = new EquipmentPlacementRepository();
        equipmentItemRepository.Update(new EquipmentPlacement("2", "2", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll()[1].Amount);
    }

    [TestMethod]
    public void TestDelete()
    {
        var equipmentItems = new List<EquipmentPlacement>
        {
            new("2", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipmentItemRepository = new EquipmentPlacementRepository();
        equipmentItemRepository.Delete(equipmentItems[1]);

        Assert.AreEqual(1, equipmentItemRepository.GetAll().Count);
        Assert.AreEqual(1, equipmentItemRepository.GetAll()[0].Amount);
        Assert.AreEqual("1", equipmentItemRepository.GetAll()[0].RoomId);
    }

    [TestMethod]
    public void TestGetAllJoinWithEquipment()
    {
        var equipmentItems = new List<EquipmentPlacement>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.Furniture),
            new("2", "Operating table",
                Equipment.EquipmentType.OperationEquipment),
        };

        Serializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");

        var loadedEquipmentPlacements = new EquipmentPlacementRepository().GetAll();

        Assert.AreEqual(2, loadedEquipmentPlacements.Count);
        Assert.IsNotNull(loadedEquipmentPlacements[0].Equipment);
        Assert.IsNotNull(loadedEquipmentPlacements[1].Equipment);
        Assert.AreEqual("Chair", loadedEquipmentPlacements[0].Equipment?.Name);
        Assert.AreEqual("Operating table", loadedEquipmentPlacements[1].Equipment?.Name);


    }
}