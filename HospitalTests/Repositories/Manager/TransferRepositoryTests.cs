using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace HospitalTests.Models.Manager;

[TestClass]
public class TransferRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        EquipmentRepository.Instance.DeleteAll();
        RoomRepository.Instance.DeleteAll();
        EquipmentPlacementRepository.Instance.DeleteAll();
        TransferRepository.Instance.DeleteAll();
        TransferItemRepository.Instance.DeleteAll();
    }

    [TestMethod]
    public void TestGetAllAndJoin()
    {
        var origin = new Room("Warehouse", Room.RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", Room.RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", Room.RoomType.ExaminationRoom);
        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var injection = new Equipment("Injection", Equipment.EquipmentType.DynamicEquipment);
        EquipmentRepository.Instance.Add(injection);

        var transfer1 = new Transfer(origin, destination1, DateTime.Now);
        var transfer2 = new Transfer(origin, destination2, DateTime.Now);
        transfer1.AddItem(new TransferItem(injection, 10));
        transfer2.AddItem(new TransferItem(injection, 4));

        TransferRepository.Instance.Add(transfer1);
        TransferRepository.Instance.Add(transfer2);

        TransferRepository.Instance.ForceFileReadOnNextCommand();
        Assert.AreEqual(2, TransferRepository.Instance.GetAll().Count);
        Assert.AreEqual(1, TransferRepository.Instance.GetAll()[1].Items.Count);
        Assert.IsNotNull(TransferRepository.Instance.GetAll()[1].Items[0].Equipment);
        Assert.IsNotNull(TransferRepository.Instance.GetAll()[0].Origin);
        Assert.AreEqual("Injection", TransferRepository.Instance.GetAll()[1].Items[0].Equipment.Name);
    }

    [TestMethod()]
    public void TestAdd()
    {
        TransferRepository.Instance.Add(new Transfer());
        Assert.AreEqual(1, TransferRepository.Instance.GetAll().Count);
    }

    [TestMethod()]
    public void TestUpdate()
    {
        var origin = new Room("Warehouse", Room.RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", Room.RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", Room.RoomType.ExaminationRoom);
        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var injection = new Equipment("Injection", Equipment.EquipmentType.DynamicEquipment);
        EquipmentRepository.Instance.Add(injection);

        var transfer1 = new Transfer(origin, destination1, DateTime.Now);
        var transfer2 = new Transfer(origin, destination2, DateTime.Now);
        transfer1.AddItem(new TransferItem(injection, 10));
        transfer2.AddItem(new TransferItem(injection, 4));

        TransferRepository.Instance.Add(transfer1);
        TransferRepository.Instance.Add(transfer2);

        TransferRepository.Instance.ForceFileReadOnNextCommand();
        Assert.AreEqual(2, TransferRepository.Instance.GetAll().Count);
        transfer2.TryDeliver(); // Will fail
        transfer2.Items[0].Amount = 2;
        TransferRepository.Instance.Update(transfer2);

        TransferRepository.Instance.ForceFileReadOnNextCommand();
        Assert.AreEqual(2, TransferRepository.Instance.GetAll().Count);
        var changedTransfer = TransferRepository.Instance.GetAll()[1];
        Assert.IsTrue(changedTransfer.Failed);
        Assert.AreEqual(2, changedTransfer.Items[0].Amount);
    }
}