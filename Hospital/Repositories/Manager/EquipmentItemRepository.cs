using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class EquipmentItemRepository
{
    private const string FilePath = "../../../Data/equipmentItems.csv";

    private static void JoinWithEquipment(List<EquipmentItem> equipmentItems)
    { 
        var allEquipment = new EquipmentRepository().GetAll();

        foreach (var equipmentItem in equipmentItems)
        {
            equipmentItem.Equipment = allEquipment.Find(equipment => equipment.Id == equipmentItem.EquipmentId);
        }

    }

    public List<EquipmentItem> GetAll()
    {
        var equipmentItems = Serializer<EquipmentItem>.FromCSV(FilePath);
        JoinWithEquipment(equipmentItems);
        return equipmentItems;
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

        var indexToUpdate = equipmentItems.FindIndex(e =>
            e.EquipmentId == equipmentItem.EquipmentId && e.RoomId == equipmentItem.RoomId);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        equipmentItems[indexToUpdate] = equipmentItem;

        Serializer<EquipmentItem>.ToCSV(equipmentItems, FilePath);
    }

    public void Delete(EquipmentItem equipmentItem)
    {
        var equipmentItems = GetAll();

        var indexToDelete = equipmentItems.FindIndex(e =>
            e.EquipmentId == equipmentItem.EquipmentId && e.RoomId == equipmentItem.RoomId);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        equipmentItems.RemoveAt(indexToDelete);

        Serializer<EquipmentItem>.ToCSV(equipmentItems, FilePath);
    }
}