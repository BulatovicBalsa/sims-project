using Hospital.Models.Manager;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Manager
{
    public class EquipmentItemRepository
    {
        private const string FilePath = "../../../Data/equipmentItems.csv";

        public List<EquipmentItem> GetAll()
        {
            return Serializer<EquipmentItem>.FromCSV(FilePath);
        }

        public void Add(EquipmentItem equipmentItem)
        {
            var equipmentItems = GetAll();
            
            equipmentItems.Add(equipmentItem);

            Serializer<EquipmentItem>.ToCSV(equipmentItems, FilePath);

        }

        public void Update(EquipmentItem equipmentItem)
        {
            var equipmentItems = GetAll();

            var indexToUpdate = equipmentItems.FindIndex(e => e.EquipmentId == equipmentItem.EquipmentId && e.RoomId == equipmentItem.RoomId);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            equipmentItems[indexToUpdate] = equipmentItem;

            Serializer<EquipmentItem>.ToCSV(equipmentItems, FilePath);
        }

        public void Delete(EquipmentItem equipmentItem)
        {
            var equipmentItems = GetAll();

            var indexToDelete = equipmentItems.FindIndex(e => e.EquipmentId == equipmentItem.EquipmentId && e.RoomId == equipmentItem.RoomId);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            equipmentItems.RemoveAt(indexToDelete);

            Serializer<EquipmentItem>.ToCSV(equipmentItems, FilePath);
            
        }

    }
}
