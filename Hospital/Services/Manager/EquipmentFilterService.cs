using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        private EquipmentRepository _equipmentRepository;
        public EquipmentFilterService()
        {
            _roomRepository = new RoomRepository();
            _equipmentPlacementRepository = EquipmentPlacementRepository.Instance;
            _equipmentRepository = EquipmentRepository.Instance;
        }

        public List<Equipment> GetEquipment(Room.RoomType type)
        {
            var result = new HashSet<Equipment>();
            var rooms = _roomRepository.GetAll();

            foreach (var room in rooms.Where(room => room.Type == type))
            {
                result.UnionWith(room.GetEquipment());
            }

            return result.ToList();
        }

        public List<Equipment> GetEquipment(Equipment.EquipmentType type)
        {
            return _equipmentRepository.GetAll().Where(equipment => equipment.Type == type).ToList();
        }

        public List<Equipment> Select(List<EquipmentPlacement> equipmentPlacements, int maxAmountInclusive)
        {
            var equipmentPlacementsByEquipment =
                from equipmentPlacement in equipmentPlacements
                group equipmentPlacement by equipmentPlacement.Equipment
                into equipmentPlacementGroup
                select equipmentPlacementGroup;

            var equipmentWithAppropriateAmounts = (from equipmentPlacementGroup in equipmentPlacementsByEquipment
                where TotalAmount(equipmentPlacementGroup) < maxAmountInclusive
                select equipmentPlacementGroup.Key).ToList();

            return equipmentWithAppropriateAmounts;

        }

        private static int TotalAmount(IGrouping<Equipment, EquipmentPlacement> equipmentPlacementGroup)
        {
            return equipmentPlacementGroup.ToList().Sum(equipmentPlacement => equipmentPlacement.Amount);
        }

        public List<Equipment> GetEquipment(int maxAmountInclusive)
        {
            var allEquipment = _equipmentRepository.GetAll();
            var rooms = _roomRepository.GetAll();
            var result = new List<Equipment>();

            foreach (var equipment in allEquipment)
            {
                if (rooms.Sum(room => room.GetAmount(equipment)) <= maxAmountInclusive) result.Add(equipment);

            }

            return result;
        }

        public List<Equipment> GetEquipmentNotInWarehouse()
        {
            var allEquipment = _equipmentRepository.GetAll();
            

            return SelectNotInWarehouse(allEquipment);

        }

        public List<Equipment> SelectNotInWarehouse(List<Equipment> equipment)
        {
            var warehouse = _roomRepository.GetAll().Find(room => room.Type == Room.RoomType.Warehouse);
            var result = new List<Equipment>();

            if (warehouse == null)
            {
                return equipment;
            }

            foreach (var e in equipment)
            {
                if (warehouse.GetAmount(e) == 0) result.Add(e);

            }

            return result;
        }

        private string GetStringRepresentation(Equipment.EquipmentType type)
        {
            switch (type)
            {
                case(Equipment.EquipmentType.ExaminationEquipment):
                return "examination equipment";
                case (Equipment.EquipmentType.Furniture):
                    return "furniture";
                case (Equipment.EquipmentType.HallwayEquipment):
                    return "hallway equipment";
                case (Equipment.EquipmentType.OperationEquipment):
                    return "operation equipment";
                default:
                    return "";

            }
        }

        public List<Equipment> Select(List<Equipment> equipment, string searchTerm)
        {
            var caseInsensitiveSearchTerm = searchTerm.ToLower();

            return equipment.Where(e =>
                e.Name.ToLower().Contains(caseInsensitiveSearchTerm) ||
                GetStringRepresentation(e.Type).Contains(caseInsensitiveSearchTerm)).ToList();
        }

        
    }
}
