using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager
{
    public class RoomFilterService
    {
        private const int lowEquipmentThreshold = 5;

        public static List<Room> GetRoomsLowOnDynamicEquipment()
        {
            var dynamicEquipment = EquipmentRepository.Instance.GetDynamic();
            var rooms = RoomRepository.Instance.GetAll();
            return rooms.Where(room =>
                dynamicEquipment.Exists(equipment => room.GetAmount(equipment) < lowEquipmentThreshold)).ToList();
        }

        public List<Room> GetRoomsForExamination()
        {
            var allRooms = RoomRepository.Instance.GetAll();
            return allRooms.Where(room =>
                room.Type is RoomType.OperatingRoom or RoomType.ExaminationRoom).ToList();
        }
    }
}
