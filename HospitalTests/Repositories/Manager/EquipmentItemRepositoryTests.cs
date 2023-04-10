using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Repositories.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager
{
    [TestClass()]
    public class EquipmentItemRepositoryTests
    {
        [TestMethod()]
        public void TestGetAll()
        {
            var equipmentItems = new List<EquipmentItem>()
            {
                new("1", "1", 1),
                new("2", "2", 2)
            };
            Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

            Assert.AreEqual(2, new EquipmentItemRepository().GetAll().Count);
        }

        [TestMethod()]
        public void TestAdd()
        {
            var equipmentItems = new List<EquipmentItem>()
            {
                new("1", "1", 1),
                new("2", "2", 2)
            };
            Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

            var equipmentItemRepository = new EquipmentItemRepository();
            equipmentItemRepository.Add(new EquipmentItem("3", "3", 3));

            Assert.AreEqual(3, equipmentItemRepository.GetAll().Count);
        }

        [TestMethod()]
        public void TestUpdate()
        {
            var equipmentItems = new List<EquipmentItem>()
            {
                new("2", "1", 1),
                new("2", "2", 2)
            };
            Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

            var equipmentItemRepository = new EquipmentItemRepository();
            equipmentItemRepository.Update(new EquipmentItem("2", "2", 3));

            Assert.AreEqual(3, equipmentItemRepository.GetAll()[1].Amount);
        }

        [TestMethod()]
        public void TestDelete()
        {
            var equipmentItems = new List<EquipmentItem>()
            {
                new("2", "1", 1),
                new("2", "2", 2)
            };
            Serializer<EquipmentItem>.ToCSV(equipmentItems, "../../../Data/equipmentItems.csv");

            var equipmentItemRepository = new EquipmentItemRepository();
            equipmentItemRepository.Delete(equipmentItems[1]);

            Assert.AreEqual(1, equipmentItemRepository.GetAll().Count);
            Assert.AreEqual(1, equipmentItemRepository.GetAll()[0].Amount);
            Assert.AreEqual("1", equipmentItemRepository.GetAll()[0].RoomId);
         }
    }
}