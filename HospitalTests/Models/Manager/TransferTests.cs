using Hospital.PhysicalAssets.Models;

namespace HospitalTests.Models.Manager;

[TestClass]
public class TransferTests
{
    [TestMethod]
    public void TestDelivery()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 10);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 10));

        Assert.IsTrue(transfer.TryDeliver());
        Assert.IsFalse(transfer.Failed);
        Assert.AreEqual(0, origin.GetAmount(injection));
        Assert.AreEqual(10, destination.GetAmount(injection));
    }

    [TestMethod]
    public void TestDoubleDelivery()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 10);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 4));

        Assert.IsTrue(transfer.TryDeliver());
        Assert.IsFalse(transfer.TryDeliver());
        Assert.AreEqual(6, origin.GetAmount(injection));
        Assert.AreEqual(4, destination.GetAmount(injection));
    }

    [TestMethod]
    public void TestTransferNotEnoughEquipmentInOrigin()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 5);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 6));

        Assert.IsFalse(transfer.TryDeliver());
        Assert.IsTrue(transfer.Failed);
        Assert.AreEqual(5, origin.GetAmount(injection));
        Assert.AreEqual(0, destination.GetAmount(injection));
    }

    [TestMethod]
    public void TestRedeliverFailedTransfer()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 4);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 5));

        Assert.IsFalse(transfer.TryDeliver());
        Assert.IsTrue(transfer.Failed);

        origin.SetAmount(injection, 10);

        Assert.IsFalse(transfer.TryDeliver());
        Assert.AreEqual(10, origin.GetAmount(injection));
        Assert.AreEqual(0, destination.GetAmount(injection));
    }

    [TestMethod]
    public void TestDeliverTooEarly()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 10);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddSeconds(10));
        transfer.AddItem(new TransferItem(injection, 4));

        Assert.IsFalse(transfer.TryDeliver());
        Assert.AreEqual(10, origin.GetAmount(injection));
        Assert.AreEqual(0, destination.GetAmount(injection));
    }

    [TestMethod]
    public void TestTransferIsPossible()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 10);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 4));

        Assert.IsTrue(transfer.IsPossible());
    }

    [TestMethod]
    public void TestTransferIsPossibleOriginDoesNotHaveEnoughEquipment()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 1);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 4));

        Assert.IsFalse(transfer.IsPossible());
    }

    [TestMethod]
    public void TestTransferNotPossibleEquipmentReserved()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 10);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 6));
        var transferThatWontHaveEquipment = new Transfer(origin, destination, DateTime.Now);
        transferThatWontHaveEquipment.AddItem(new TransferItem(injection, 6));

        origin.TryReserveEquipment(transfer);
        Assert.IsFalse(transferThatWontHaveEquipment.IsPossible());
    }

    [TestMethod]
    public void TestTransferIsPossibleEquipmentReserved()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination = new Room("Examination room", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        origin.SetAmount(injection, 10);

        var transfer = new Transfer(origin, destination, DateTime.Now.AddDays(-1));
        transfer.AddItem(new TransferItem(injection, 6));
        var transfer2 = new Transfer(origin, destination, DateTime.Now);
        transfer2.AddItem(new TransferItem(injection, 4));

        origin.TryReserveEquipment(transfer);
        Assert.IsTrue(transfer2.IsPossible());
    }
}