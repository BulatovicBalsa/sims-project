using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager
{
    public class EquipmentFilterService
    {
        private RoomRepository _roomRepository;
        private EquipmentPlacementRepository _equipmentPlacementRepository;

        public EquipmentFilterService()
        {
            _roomRepository = new RoomRepository();
            _equipmentPlacementRepository = new EquipmentPlacementRepository();
        }

        public List<Equipment> GetEquipmentInRoomType(Room.RoomType type)
        {
            var result = new HashSet<Equipment>();
            var rooms = _roomRepository.GetAll();

            foreach (var room in rooms.Where(room => room.Type == type))
            {
                result.UnionWith(room.GetEquipment());
            }

            return result.ToList();
        }
    }
}
