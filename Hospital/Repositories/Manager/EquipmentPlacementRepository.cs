using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class EquipmentPlacementRepository
{
    private const string FilePath = "../../../Data/equipmentItems.csv";

    private static EquipmentPlacementRepository? _instance;

    private List<EquipmentPlacement>? _equipmentPlacements;

    private EquipmentPlacementRepository() { }

    public static EquipmentPlacementRepository Instance => _instance ??= new EquipmentPlacementRepository();

    private static void JoinWithEquipment(List<EquipmentPlacement> equipmentItems)
    {
        var allEquipment = EquipmentRepository.Instance.GetAll();

        foreach (var equipmentPlacement in equipmentItems)
            equipmentPlacement.Equipment =
                allEquipment.Find(equipment => equipment.Id == equipmentPlacement.EquipmentId);
    }

    public List<EquipmentPlacement> GetAll()
    {
        if (_equipmentPlacements != null) return _equipmentPlacements;
        _equipmentPlacements = Serializer<EquipmentPlacement>.FromCSV(FilePath);
        JoinWithEquipment(_equipmentPlacements);

        return _equipmentPlacements;
    }

    public EquipmentPlacement? GetByKey(string roomId, string equipmentId)
    {
        return GetAll().Find(e => e.RoomId == roomId && e.EquipmentId == equipmentId);
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

        var indexToUpdate = equipmentPlacements.FindIndex(e => e.Equals(equipmentPlacement));
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        equipmentPlacements[indexToUpdate] = equipmentPlacement;

        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, FilePath);
    }

    public void Delete(EquipmentPlacement equipmentPlacement)
    {
        var equipmentPlacements = GetAll();

        var indexToDelete = equipmentPlacements.FindIndex(e => e.Equals(equipmentPlacement));
        if (indexToDelete == -1) throw new KeyNotFoundException();

        equipmentPlacements.RemoveAt(indexToDelete);

        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, FilePath);
    }

    public void DeleteAll()
    {
        var equipmentPlacements = _equipmentPlacements ?? GetAll();
        equipmentPlacements.Clear();
        Serializer<EquipmentPlacement>.ToCSV(equipmentPlacements, FilePath);
        _equipmentPlacements = null;
    }
}