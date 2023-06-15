using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.Scheduling;

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

    [TestMethod]
    public void TestWillExist()
    {
        var room = new Room();
        room.CreationDate = DateTime.Now.AddDays(-1);
        room.DemolitionDate = DateTime.Now.AddDays(2);
        Assert.IsFalse(room.WillExist(DateTime.Now.AddDays(-2)));
    }

    [TestMethod]
    public void TestWillExistDuring()
    {
        var room = new Room();
        room.CreationDate = DateTime.Now.AddDays(-1);
        room.DemolitionDate = DateTime.Now.AddDays(2);

        Assert.IsTrue(room.WillExistDuring(new TimeRange(DateTime.Now, DateTime.Now.AddDays(1))));
    }

    [TestMethod]
    public void TestWillExistDuringDemolishedBeforeEnd()
    {
        var room = new Room();
        room.CreationDate = DateTime.Now.AddDays(-1);
        room.DemolitionDate = DateTime.Now.AddDays(2);

        Assert.IsFalse(room.WillExistDuring(new TimeRange(DateTime.Now, DateTime.Now.AddDays(10))));
    }

    [TestMethod]
    public void TestWillExistDuringCreatedAfterBegin()
    {
        var room = new Room();
        room.CreationDate = DateTime.Now.AddDays(-1);
        room.DemolitionDate = DateTime.Now.AddDays(2);

        Assert.IsFalse(room.WillExistDuring(new TimeRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(1))));
    }
}