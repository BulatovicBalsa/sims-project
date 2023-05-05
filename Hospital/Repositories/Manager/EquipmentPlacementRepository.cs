using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class EquipmentPlacementRepository
{
    private const string FilePath = "../../../Data/equipmentItems.csv";

    private static EquipmentPlacementRepository? _instance;

    private List<EquipmentPlacement>? _equipmentPlacements;

    private EquipmentPlacementRepository()
    {
    }

    public static EquipmentPlacementRepository Instance
    {
        get
        {
            if (_instance == null) _instance = new EquipmentPlacementRepository();

            return _instance;
        }
    }

    private static void JoinWithEquipment(List<EquipmentPlacement> equipmentItems)
    {
        var allEquipment = EquipmentRepository.Instance.GetAll();

        foreach (var equipmentPlacement in equipmentItems)
            equipmentPlacement.Equipment =
                allEquipment.Find(equipment => equipment.Id == equipmentPlacement.EquipmentId);
    }

    public List<EquipmentPlacement> GetAll()
    {
        if (_equipmentPlacements == null)
        {
            _equipmentPlacements = Serializer<EquipmentPlacement>.FromCSV(FilePath);
            JoinWithEquipment(_equipmentPlacements);
        }

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

    public void DeleteAll()
    {
        if (_equipmentPlacements == null) return;
        _equipmentPlacements.Clear(); // TODO: Remove this warning and other similar ones
        Serializer<EquipmentPlacement>.ToCSV(_equipmentPlacements, FilePath);
        _equipmentPlacements = null;
    }
}