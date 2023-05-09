using Hospital.Models.Manager;

namespace HospitalTests.Models.Manager;

[TestClass]
public class RoomTests
{
    [TestMethod]
    public void TestExpendEquipmentMoreThanAvailable()
    {
        var room = new Room("Room", RoomType.ExaminationRoom);
        var equipment = new Equipment("Dynamic equipment", EquipmentType.DynamicEquipment);
        room.SetAmount(equipment, 10);

        room.ExpendEquipment(equipment, 11);
        Assert.AreEqual(10, room.GetAmount(equipment));
    }

    [TestMethod]
    public void TestExpendAllEquipment()
    {
        var room = new Room("Room", RoomType.ExaminationRoom);
        var equipment = new Equipment("Dynamic equipment", EquipmentType.DynamicEquipment);
        room.SetAmount(equipment, 10);

        room.ExpendEquipment(equipment, 10);
        Assert.AreEqual(0, room.GetAmount(equipment));
    }

    [TestMethod]
    public void TestExpendEquipment()
    {
        var room = new Room("Room", RoomType.ExaminationRoom);
        var equipment = new Equipment("Dynamic equipment", EquipmentType.DynamicEquipment);
        room.SetAmount(equipment, 10);

        room.ExpendEquipment(equipment, 6);
        Assert.AreEqual(4, room.GetAmount(equipment));
    }
}