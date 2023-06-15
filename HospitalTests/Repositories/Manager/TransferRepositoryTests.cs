using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class TransferRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        var transferItemsFile = "../../../Data/transferItems.json";
        var transfersFile = "../../../Data/transfers.csv";

        if (File.Exists(transferItemsFile))
            File.Delete(transferItemsFile);
        if (File.Exists(transfersFile))
            File.Delete(transfersFile);

        new TransferItemRepository(SerializerInjector.CreateInstance<ISerializer<TransferItem>>()).DeleteAll();
        TransferRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();
        RoomRepository.Instance.DeleteAll();
        InventoryItemRepository.Instance.DeleteAll();
    }

    [TestMethod]
    public void TestGetAllAndJoin()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", RoomType.ExaminationRoom);
        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);
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

    public void TestAdd()
    {
        TransferRepository.Instance.Add(new Transfer());
        Assert.AreEqual(1, TransferRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var origin = new Room("Warehouse", RoomType.Warehouse);
        var destination1 = new Room("Examination room 1", RoomType.ExaminationRoom);
        var destination2 = new Room("Examination room 2", RoomType.ExaminationRoom);
        RoomRepository.Instance.Add(origin);
        RoomRepository.Instance.Add(destination1);
        RoomRepository.Instance.Add(destination2);

        var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);
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