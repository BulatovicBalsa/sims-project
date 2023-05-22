using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Repositories.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;

namespace HospitalTests.Models.Manager
{
    [TestClass()]
    public class RenovationRepositoryTests
    {
        public const string roomFilePath = "../../../Data/rooms.csv";
        public const string renovationFilePath = "../../../Data/renovations.csv";

        [TestInitialize()]
        public void SetUp()
        {
            if (File.Exists(roomFilePath))
            {
                File.Delete(roomFilePath);
            }

            if (File.Exists(renovationFilePath))
            {
                File.Delete(renovationFilePath);
            }

            RoomRepository.Instance.Add(new Room("1", "Warehouse", RoomType.Warehouse));
            RoomRepository.Instance.Add(new Room("2", "Ward", RoomType.Ward));
        }

        [TestMethod]
        public void TestGetAll()
        {
            File.WriteAllText(renovationFilePath, "Id,RoomId,BeginTime,EndTime,Completed,Id,Name,Type\r\n84d9612d-b5c8-4362-b446-294d49997119,1,05/22/2023 08:13:56,05/22/2023 08:13:56,False,1,Warehouse,Warehouse\r\n8fb9bdec-2662-42e0-a408-c4cedc86d1ba,2,05/22/2023 08:13:56,05/22/2023 08:13:56,False,2,Ward,Ward");
            Assert.AreEqual(2, RenovationRepository.Instance.GetAll().Count);
        }

        [TestMethod()]
        public void TestAdd()
        {
            RenovationRepository.Instance.Add(new Renovation("1", DateTime.Now, DateTime.Now, RoomRepository.Instance.GetById("1") ?? throw new InvalidOperationException()));
            RenovationRepository.Instance.Add(new Renovation("2", DateTime.Now, DateTime.Now, RoomRepository.Instance.GetById("2") ?? throw new InvalidOperationException()));
            Assert.AreEqual(2, RenovationRepository.Instance.GetAllFromFile().Count);
        }
    }
}