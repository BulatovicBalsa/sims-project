using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace HospitalTests.Models.Manager;

[TestClass]
public class RoomFilterServiceTests
{
    [TestInitialize]
    public void SetUp()
    {
        InventoryItemRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();
        RoomRepository.Instance.DeleteAll();
    }

    [TestMethod]
    public void TestGetRoomsLowOnDynamicEquipment()
    {
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);
        var paper = new Equipment("Paper", EquipmentType.DynamicEquipment);
        var chair = new Equipment("Chair", EquipmentType.Furniture);

        var room1 = new Room("Room 1", RoomType.Ward);
        var room2 = new Room("Room 2", RoomType.ExaminationRoom);
        var room3 = new Room("Room 3", RoomType.WaitingRoom);
        var room4 = new Room("Room 4", RoomType.OperatingRoom);

        room1.SetAmount(chair, 10);

        room2.SetAmount(injection, 5);
        room2.SetAmount(paper, 15);

        room3.SetAmount(injection, 4);
        room3.SetAmount(paper, 10);

        room4.SetAmount(chair, 10);
        room4.SetAmount(injection, 4);
        room4.SetAmount(paper, 3);

        EquipmentRepository.Instance.Add(injection);
        EquipmentRepository.Instance.Add(paper);
        EquipmentRepository.Instance.Add(chair);

        RoomRepository.Instance.Add(room1);
        RoomRepository.Instance.Add(room2);
        RoomRepository.Instance.Add(room3);
        RoomRepository.Instance.Add(room4);

        var roomsLowOnDynamicEquipment = RoomFilterService.GetRoomsLowOnDynamicEquipment();
        Assert.AreEqual(3, roomsLowOnDynamicEquipment.Count);
        Assert.IsTrue(roomsLowOnDynamicEquipment.Exists(room => room.Equals(room1)));
        Assert.IsTrue(roomsLowOnDynamicEquipment.Exists(room => room.Equals(room3)));
        Assert.IsTrue(roomsLowOnDynamicEquipment.Exists(room => room.Equals(room4)));
    }
}