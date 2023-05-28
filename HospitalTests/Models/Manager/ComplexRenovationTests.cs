using Hospital.Models.Manager;
using Hospital.Scheduling;

namespace HospitalTests.Models.Manager;

[TestClass]
public class ComplexRenovationTests
{
    [TestMethod]
    public void TestSchedule()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom)
        };
        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));

        var complexRenovation = ComplexRenovation.Schedule(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });

        foreach (var room in toDemolish) Assert.AreEqual(complexRenovation.EndTime, room.DemolitionDate);

        foreach (var room in toBuild) Assert.AreEqual(complexRenovation.EndTime, room.CreationDate);
    }

    [TestMethod]
    public void TestTryComplete()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom)
        };
        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        var otherEquipment = new Equipment("Other", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(otherEquipment, 3);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));

        var complexRenovation = ComplexRenovation.Schedule(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });

        Assert.IsTrue(complexRenovation.TryComplete());

        Assert.AreEqual(10, toBuild[0].GetAmount(equipment));
        Assert.AreEqual(3, toBuild[0].GetAmount(otherEquipment));
        Assert.AreEqual(0, toDemolish[0].GetAmount(equipment));
        Assert.AreEqual(0, toDemolish[1].GetAmount(otherEquipment));
    }

    [TestMethod]
    public void TestGetSimpleRenovations()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom)
        };
        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));

        var complexRenovation = ComplexRenovation.Schedule(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });

        var simpleRenovations = complexRenovation.GetSimpleRenovations();

        Assert.AreEqual(3, simpleRenovations.Count);

        foreach (var renovation in simpleRenovations)
        {
            Assert.AreEqual(complexRenovation.BeginTime, renovation.BeginTime);
            Assert.AreEqual(complexRenovation.EndTime, renovation.EndTime);
        }
    }
}