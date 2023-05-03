using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class EquipmentPlacementRepository
{
    private const string FilePath = "../../../Data/equipmentItems.csv";

    private static void JoinWithEquipment(List<EquipmentPlacement> equipmentItems)
    { 
        var allEquipment = EquipmentRepository.Instance.GetAll();

        foreach (var equipmentPlacement in equipmentItems)
        {
            equipmentPlacement.Equipment = allEquipment.Find(equipment => equipment.Id == equipmentPlacement.EquipmentId);
        }

    }

    public List<EquipmentPlacement> GetAll()
    {
        var equipmentPlacements = Serializer<EquipmentPlacement>.FromCSV(FilePath);
        JoinWithEquipment(equipmentPlacements);
        return equipmentPlacements;
    }

    public void Add(EquipmentPlacement equipmentPlacement)
    {
        var equipmentPlacements = GetAll();

        equipmentPlacements.Add(equipmentPlacement);

        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, FilePath);
    }

    public void Update(EquipmentPlacement equipmentPlacement)
    {
        var equipmentPlacements = GetAll();

        var indexToUpdate = equipmentPlacements.FindIndex(e =>
            e.EquipmentId == equipmentPlacement.EquipmentId && e.RoomId == equipmentPlacement.RoomId);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        equipmentPlacements[indexToUpdate] = equipmentPlacement;

        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, FilePath);
    }

    public void Delete(EquipmentPlacement equipmentPlacement)
    {
        var equipmentPlacements = GetAll();

        var indexToDelete = equipmentPlacements.FindIndex(e =>
            e.EquipmentId == equipmentPlacement.EquipmentId && e.RoomId == equipmentPlacement.RoomId);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        equipmentPlacements.RemoveAt(indexToDelete);

        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, FilePath);
    }
}