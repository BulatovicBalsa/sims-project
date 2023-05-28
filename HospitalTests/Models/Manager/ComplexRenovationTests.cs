using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hospital.Scheduling;

namespace HospitalTests.Models.Manager
{
    [TestClass()]
    public class ComplexRenovationTests
    {
        [TestMethod()]
        public void TestSchedule()
        {
            List<Room> toDemolish = new List<Room>
            {
                new("Examination room", RoomType.ExaminationRoom),
                new("Ward", RoomType.Ward)
            };
            List<Room> toBuild = new List<Room>
            {
                new Room("Operating room", RoomType.OperatingRoom)
            };
            Equipment equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
            toDemolish[0].SetAmount(equipment, 10);
            Transfer fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
            fromOldToNew.AddItem(new TransferItem(equipment, 8));

            ComplexRenovation.Schedule(toDemolish, toBuild, new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
                toBuild[0], new List<Transfer>{fromOldToNew});

            foreach (var room in toDemolish)
            {
                Assert.AreEqual(DateTime.Now, room.DemolitionDate);
            }

            foreach (var room in toBuild)
            {
               Assert.AreEqual(DateTime.Now, room.CreationDate); 
            }
        }
    }
}