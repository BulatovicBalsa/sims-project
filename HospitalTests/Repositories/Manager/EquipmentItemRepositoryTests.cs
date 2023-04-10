using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;
using NuGet.Frameworks;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentItemRepositoryTests
{
    [TestMethod]
    public void TestGetAll()
    {
        var equipmentItems = new List<EquipmentItem>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        Assert.AreEqual(2, new EquipmentItemRepository().GetAll().Count);
    }

    [TestMethod]
    public void TestAdd()
    {
        var equipmentItems = new List<EquipmentItem>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipmentItemRepository = new EquipmentItemRepository();
        equipmentItemRepository.Add(new EquipmentItem("3", "3", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll().Count);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentItems = new List<EquipmentItem>
        {
            new("2", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipmentItemRepository = new EquipmentItemRepository();
        equipmentItemRepository.Update(new EquipmentItem("2", "2", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll()[1].Amount);
    }

    [TestMethod]
    public void TestDelete()
    {
        var equipmentItems = new List<EquipmentItem>
        {
            new("2", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipmentItemRepository = new EquipmentItemRepository();
        equipmentItemRepository.Delete(equipmentItems[1]);

        Assert.AreEqual(1, equipmentItemRepository.GetAll().Count);
        Assert.AreEqual(1, equipmentItemRepository.GetAll()[0].Amount);
        Assert.AreEqual("1", equipmentItemRepository.GetAll()[0].RoomId);
    }

    public void TestGetAllJoinWithEquipment()
    {
        var equipmentItems = new List<EquipmentItem>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
        };

        Serializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");

        var loadedEquipmentItems = new EquipmentItemRepository().GetAll();

        Assert.AreEqual(2, loadedEquipmentItems.Count);
        Assert.IsNotNull(loadedEquipmentItems[0].Equipment);
        Assert.IsNotNull(loadedEquipmentItems[1].Equipment);
        Assert.Equals("Chair", loadedEquipmentItems[0].Equipment?.Name);
        Assert.Equals("Operating table", loadedEquipmentItems[1].Equipment?.Name);


    }
}