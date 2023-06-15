using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.Core.PhysicalAssets.Services;
using Hospital.Injectors;
using Hospital.Serialization;

namespace HospitalTests.Services.Manager;

[TestClass]
public class TransferServiceTests
{
    [TestInitialize]
    public void SetUp()
    {
        EquipmentRepository.Instance.DeleteAll();
        RoomRepository.Instance.DeleteAll();
        InventoryItemRepository.Instance.DeleteAll();
        TransferRepository.Instance.DeleteAll();
        new TransferItemRepository(SerializerInjector.CreateInstance<ISerializer<TransferItem>>()).DeleteAll();
    }

    [TestMethod]
    public void TestTrySendTransfer()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        EquipmentRepository.Instance.Add(injection);
        origin.SetAmount(injection, 10);

        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var transferItems = new List<TransferItem>
        {
            new(injection, 6)
        };

        Assert.IsTrue(TransferService.TrySendTransfer(origin, destination1, transferItems, DateTime.Now.AddDays(1)));
        Assert.AreEqual(10, origin.GetAmount(injection));
        Assert.AreEqual(1, TransferRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestTrySendTransferAllEquipmentReserved()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        EquipmentRepository.Instance.Add(injection);
        origin.SetAmount(injection, 10);

        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var transferItems1 = new List<TransferItem>
        {
            new(injection, 6)
        };

        var transferItems2 = new List<TransferItem>
        {
            new(injection, 4)
        };

        Assert.IsTrue(TransferService.TrySendTransfer(origin, destination1, transferItems1, DateTime.Now.AddDays(1)));
        Assert.AreEqual(10, origin.GetAmount(injection));
        Assert.AreEqual(1, TransferRepository.Instance.GetAll().Count);
        Assert.IsTrue(TransferService.TrySendTransfer(origin, destination2, transferItems2, DateTime.Now.AddDays(1)));
        Assert.IsFalse(TransferService.TrySendTransfer(origin, destination1, transferItems2, DateTime.Now.AddDays(1)));
    }

    [TestMethod]
    public void TestFreeReserved()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", RoomType.ExaminationRoom);
        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

        EquipmentRepository.Instance.Add(injection);
        origin.SetAmount(injection, 10);

        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var transferItems1 = new List<TransferItem>
        {
            new(injection, 6)
        };

        Assert.IsTrue(
            TransferService.TrySendTransfer(origin, destination1, transferItems1, DateTime.Now.AddSeconds(-1)));
        TransferService.AttemptDeliveryOfAllTransfers();
        Thread.Sleep(5000);
        Assert.IsTrue(TransferRepository.Instance.GetAll()[0].Delivered);
        Assert.AreEqual(0, origin.Inventory[0].Reserved);
    }
}