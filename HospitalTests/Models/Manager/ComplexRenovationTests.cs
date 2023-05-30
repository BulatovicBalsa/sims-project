﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });

        complexRenovation.Schedule();

        foreach (var room in toDemolish)
            Assert.AreEqual(complexRenovation.EndTime, room.DemolitionDate, "Demolition date is not correct");

        foreach (var room in toBuild)
            Assert.AreEqual(complexRenovation.EndTime, room.CreationDate, "Creation date is not correct");
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

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });
        complexRenovation.Schedule();

        Assert.IsTrue(complexRenovation.TryComplete());

        Assert.AreEqual(10, toBuild[0].GetAmount(equipment));
        Assert.AreEqual(3, toBuild[0].GetAmount(otherEquipment));
        Assert.AreEqual(0, toDemolish[0].GetAmount(equipment));
        Assert.AreEqual(0, toDemolish[1].GetAmount(otherEquipment));
    }

    [TestMethod]
    public void TestTryCompleteNotScheduled()
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

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });

        Assert.IsFalse(complexRenovation.TryComplete(),
            "Renovation that has not been scheduled yey should not complete");
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

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });
        complexRenovation.Schedule();

        var simpleRenovations = complexRenovation.GetSimpleRenovations();

        Assert.AreEqual(3, simpleRenovations.Count);

        foreach (var renovation in simpleRenovations)
        {
            Assert.AreEqual(complexRenovation.BeginTime, renovation.BeginTime);
            Assert.AreEqual(complexRenovation.EndTime, renovation.EndTime);
        }
    }

    [TestMethod()]
    public void TestIsEquipmentProperlyRedistributed()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom),
            new("Another room", RoomType.OperatingRoom)
        };
        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        var otherEquipment = new Equipment("Something else", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(otherEquipment, 10);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));
        fromOldToNew.AddItem(new TransferItem(otherEquipment, 6));
        var fromOldToSecondNew = new Transfer(toDemolish[1], toBuild[1], DateTime.Now);
        fromOldToSecondNew.AddItem(new TransferItem(equipment, 12));
        fromOldToSecondNew.AddItem(new TransferItem(otherEquipment, 4));
        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew, fromOldToSecondNew });

        Assert.IsTrue(complexRenovation.IsEquipmentProperlyRedistributed());
    }
    [TestMethod()]
    public void TestIsEquipmentProperlyRedistributedAttemptToRedistributeMoreThanAvailable()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom),
            new("Another room", RoomType.OperatingRoom)
        };
        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        var otherEquipment = new Equipment("Something else", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(otherEquipment, 10);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));
        fromOldToNew.AddItem(new TransferItem(otherEquipment, 6));
        var fromOldToSecondNew = new Transfer(toDemolish[1], toBuild[1], DateTime.Now);
        fromOldToSecondNew.AddItem(new TransferItem(equipment, 16)); // Offending item
        fromOldToSecondNew.AddItem(new TransferItem(otherEquipment, 4));
        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew, fromOldToSecondNew });

        Assert.IsFalse(complexRenovation.IsEquipmentProperlyRedistributed());
    }

}