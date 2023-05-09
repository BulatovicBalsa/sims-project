using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentPlacementRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        EquipmentPlacementRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();
        var equipment = new List<Equipment>
        {
            new("1", "Chair", EquipmentType.Furniture),
            new("2", "Operating table",
                EquipmentType.OperationEquipment)
        };

        Serializer<Equipment>.ToCSV(equipment, "../../../Data/equipment.csv");

        var equipmentPlacements = new List<EquipmentPlacement>
        {
            new("1", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, "../../../Data/equipmentItems.csv");
    }


    [TestMethod]
    public void TestGetAll()
    {
        Assert.AreEqual(2, EquipmentPlacementRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestAdd()
    {
        var equipmentItemRepository = EquipmentPlacementRepository.Instance;
        equipmentItemRepository.Add(new EquipmentPlacement("3", "3", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll().Count);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentItemRepository = EquipmentPlacementRepository.Instance;
        equipmentItemRepository.Update(new EquipmentPlacement("2", "2", 3));

        Assert.AreEqual(3, equipmentItemRepository.GetAll()[1].Amount);
    }

    [TestMethod]
    public void TestDelete()
    {
        EquipmentPlacementRepository.Instance.DeleteAll();
        var equipmentPlacements = new List<EquipmentPlacement>
        {
            new("2", "1", 1),
            new("2", "2", 2)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, "../../../Data/equipmentItems.csv");


        var equipmentItemRepository = EquipmentPlacementRepository.Instance;
        equipmentItemRepository.Delete(equipmentPlacements[1]);

        Assert.AreEqual(1, equipmentItemRepository.GetAll().Count);
        Assert.AreEqual(1, equipmentItemRepository.GetAll()[0].Amount);
        Assert.AreEqual("1", equipmentItemRepository.GetAll()[0].RoomId);
    }

    [TestMethod]
    public void TestGetAllJoinWithEquipment()
    {
        var loadedEquipmentPlacements = EquipmentPlacementRepository.Instance.GetAll();

        Assert.AreEqual(2, loadedEquipmentPlacements.Count);
        Assert.IsNotNull(loadedEquipmentPlacements[0].Equipment);
        Assert.IsNotNull(loadedEquipmentPlacements[1].Equipment);
        Assert.AreEqual("Chair", loadedEquipmentPlacements[0].Equipment?.Name);
        Assert.AreEqual("Operating table", loadedEquipmentPlacements[1].Equipment?.Name);
    }
}