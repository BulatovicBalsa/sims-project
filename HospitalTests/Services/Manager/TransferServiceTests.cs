using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Services.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repositories.Manager;
using Hospital.Models.Manager;

namespace HospitalTests.Models.Manager
{
    [TestClass()]
    public class TransferServiceTests
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

        [TestMethod()]
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

            var transferItems = new List<TransferItem>()
            {
                new TransferItem(injection, 6)
            };

            Assert.IsTrue(TransferService.TrySendTransfer(origin, destination1, transferItems, DateTime.Now.AddDays(1)));
            Assert.AreEqual(10, origin.GetAmount(injection));
            Assert.AreEqual(1, TransferRepository.Instance.GetAll().Count);
        }

        [TestMethod]
        public void TestTrySendTransferAllEquipmentReserved()
        {
            TransferService.DisableAutomaticDelivery();
            var origin = new Room("Warehouse", RoomType.Warehouse);
            var destination1 = new Room("Examination room 1", RoomType.ExaminationRoom);
            var destination2 = new Room("Examination room 2", RoomType.ExaminationRoom);
            var injection = new Equipment("Injection", EquipmentType.DynamicEquipment);

            EquipmentRepository.Instance.Add(injection);
            origin.SetAmount(injection, 10);

            RoomRepository.Instance.Add(origin);
            RoomRepository.Instance.Add(destination1);
            RoomRepository.Instance.Add(destination2);

            var transferItems1 = new List<TransferItem>()
            {
                new TransferItem(injection, 6)
            };

            var transferItems2 = new List<TransferItem>()
            {
                new TransferItem(injection, 4)
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

            var transferItems1 = new List<TransferItem>()
            {
                new TransferItem(injection, 6)
            };

            Assert.IsTrue(TransferService.TrySendTransfer(origin, destination1, transferItems1, DateTime.Now.AddSeconds(-1)));
            TransferService.AttemptDeliveryOfAllTransfers();
            Thread.Sleep(5000);
            Assert.IsTrue(TransferRepository.Instance.GetAll()[0].Delivered);
            Assert.AreEqual(0, origin.Inventory[0].Reserved);
        }
    }
}