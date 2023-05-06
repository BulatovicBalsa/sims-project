using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Services.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using System.Threading;

namespace HospitalTests.Services.Manager
{
    [TestClass()]
    public class EquipmentOrderServiceTests
    {
        [TestMethod()]
        public void TestAttemptPickUpOfAllOrders()
        {
            EquipmentOrderRepository.Instance.DeleteAll();
            RoomRepository.Instance.DeleteAll();
            RoomRepository.Instance.Add(new Room("1", "Warehouse", Room.RoomType.Warehouse));
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);
            var orderItems = new List<EquipmentOrderItem>
            {
                new EquipmentOrderItem("1", 2, equipment)
            };

            var orderThatWillBePickedUp = new EquipmentOrder(DateTime.Now.AddDays(-1));
            orderThatWillBePickedUp.AddOrUpdateItem(equipment, 3);

            EquipmentOrderService.SendOrder(orderItems);
            EquipmentOrderService.SendOrder(orderItems);
            EquipmentOrderService.SendOrder(orderThatWillBePickedUp);
            EquipmentOrderService.AttemptPickUpOfAllOrders();

            Assert.AreEqual(3, RoomRepository.Instance.GetWarehouse().GetAmount(equipment));
        }

        [TestMethod()]
        public void TestAttemptPickUpOfAllOrdersMultipleTimes()
        {
            EquipmentOrderRepository.Instance.DeleteAll();
            RoomRepository.Instance.DeleteAll();
            RoomRepository.Instance.Add(new Room("1", "Warehouse", Room.RoomType.Warehouse));
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);
            var orderItems = new List<EquipmentOrderItem>
            {
                new EquipmentOrderItem("1", 2, equipment)
            };

            var orderThatWillBePickedUp = new EquipmentOrder(DateTime.Now.AddDays(-1));
            orderThatWillBePickedUp.AddOrUpdateItem(equipment, 3);

            EquipmentOrderService.SendOrder(orderItems);
            EquipmentOrderService.SendOrder(orderItems);
            EquipmentOrderService.SendOrder(orderThatWillBePickedUp);
            EquipmentOrderService.AttemptPickUpOfAllOrders();

            var anotherOrderThatWillBePickedUp = new EquipmentOrder(DateTime.Now.AddDays(-1));
            anotherOrderThatWillBePickedUp.AddOrUpdateItem(equipment, 3);
            EquipmentOrderService.SendOrder(anotherOrderThatWillBePickedUp);

            EquipmentOrderService.AttemptPickUpOfAllOrders(); // Will pick up only anotherOrderThatWillBePickedUp, orderThatWillBePickedUp has already been picked up

            Assert.AreEqual(6, RoomRepository.Instance.GetWarehouse().GetAmount(equipment));
        }

        [TestMethod]
        public void TestPickUpOnTimer()
        {
            EquipmentOrderRepository.Instance.DeleteAll();
            RoomRepository.Instance.DeleteAll();
            RoomRepository.Instance.Add(new Room("1", "Warehouse", Room.RoomType.Warehouse));
            
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);
            var orderItems = new List<EquipmentOrderItem>
            {
                new EquipmentOrderItem("1", 2, equipment)
            };

            var orderThatWillBePickedUp = new EquipmentOrder(DateTime.Now.AddDays(-1));
            orderThatWillBePickedUp.AddOrUpdateItem(equipment, 3);

            EquipmentOrderService.SendOrder(orderItems);
            EquipmentOrderService.SendOrder(orderItems);
            EquipmentOrderService.SendOrder(orderThatWillBePickedUp);
            
            var anotherOrderThatWillBePickedUp = new EquipmentOrder(DateTime.Now.AddSeconds(1.5));
            anotherOrderThatWillBePickedUp.AddOrUpdateItem(equipment, 3);
            EquipmentOrderService.SendOrder(anotherOrderThatWillBePickedUp);

            Thread.Sleep(1400);
            

            Assert.AreEqual(3, RoomRepository.Instance.GetWarehouse().GetAmount(equipment));
            Assert.IsTrue(orderThatWillBePickedUp.PickedUp);

            Thread.Sleep(1000);
            Assert.AreEqual(6, RoomRepository.Instance.GetWarehouse().GetAmount(equipment));
        }
    }
}