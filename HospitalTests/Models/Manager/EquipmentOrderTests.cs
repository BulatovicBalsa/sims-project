using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HospitalTests.Models.Manager
{
    [TestClass()]
    public class EquipmentOrderTests
    {
        [TestMethod]
        public void TestAddItem()
        {
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);

            var order = EquipmentOrder.CreateBlankOrder();
            order.AddOrUpdateItem(equipment, 2);

            Assert.AreEqual(2, order.GetAmount(equipment));
        } 
        [TestMethod()]
        public void TestPickUpTooEarly()
        {

            var warehouse = new Room("5000", "Warehouse Room", Room.RoomType.Warehouse);
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);
            var order = EquipmentOrder.CreateBlankOrder();
            order.AddOrUpdateItem(equipment, 2);

            order.PickUp(warehouse);

            Assert.IsFalse(order.PickedUp);
            Assert.AreEqual(0, warehouse.GetAmount(equipment));
        }
        
        [TestMethod()] 
        public void TestPickUp()
        {

            var warehouse = new Room("5000", "Warehouse Room", Room.RoomType.Warehouse);
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);
            var order = new EquipmentOrder(DateTime.Now.AddDays(-1));
            order.AddOrUpdateItem(equipment, 2);

            order.PickUp(warehouse);

            Assert.IsTrue(order.PickedUp);
            Assert.AreEqual(2, warehouse.GetAmount(equipment));
        }
        [TestMethod()] 
        public void TestDoublePickUp()
        {

            var warehouse = new Room("5000", "Warehouse Room", Room.RoomType.Warehouse);
            var equipment = new Equipment("1", "Something", Equipment.EquipmentType.DynamicEquipment);
            var order = new EquipmentOrder(DateTime.Now.AddDays(-1));
            order.AddOrUpdateItem(equipment, 2);

            order.PickUp(warehouse);
            order.PickUp(warehouse);

            Assert.IsTrue(order.PickedUp);
            Assert.AreEqual(2, warehouse.GetAmount(equipment));
        }
    }
}