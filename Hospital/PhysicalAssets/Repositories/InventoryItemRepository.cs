using System.Collections.Generic;
using Hospital.PhysicalAssets.Models;
using Hospital.Serialization;

namespace Hospital.PhysicalAssets.Repositories;

public class InventoryItemRepository
{
    private const string FilePath = "../../../Data/equipmentItems.csv";

    private static InventoryItemRepository? _instance;

    private List<InventoryItem>? _equipmentPlacements;

    private InventoryItemRepository()
    {
    }

    public static InventoryItemRepository Instance => _instance ??= new InventoryItemRepository();

    private static void JoinWithEquipment(List<InventoryItem> equipmentItems)
    {
        var allEquipment = EquipmentRepository.Instance.GetAll();

        foreach (var equipmentPlacement in equipmentItems)
            equipmentPlacement.Equipment =
                allEquipment.Find(equipment => equipment.Id == equipmentPlacement.EquipmentId);
    }

    public List<InventoryItem> GetAll()
    {
        if (_equipmentPlacements != null) return _equipmentPlacements;
        _equipmentPlacements = CsvSerializer<InventoryItem>.FromCSV(FilePath);
        JoinWithEquipment(_equipmentPlacements);

        return _equipmentPlacements;
    }

    public InventoryItem? GetByKey(string roomId, string equipmentId)
    {
        return GetAll().Find(e => e.RoomId == roomId && e.EquipmentId == equipmentId);
    }

    public void Add(InventoryItem inventoryItem)
    {
        var equipmentPlacements = GetAll();

        equipmentPlacements.Add(inventoryItem);

        CsvSerializer<InventoryItem>.ToCSV(equipmentPlacements, FilePath);
    }

    public void Update(InventoryItem inventoryItem)
    {
        var equipmentPlacements = GetAll();

        var indexToUpdate = equipmentPlacements.FindIndex(e => e.Equals(inventoryItem));
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        equipmentPlacements[indexToUpdate] = inventoryItem;

        CsvSerializer<InventoryItem>.ToCSV(equipmentPlacements, FilePath);
    }

    public void Delete(InventoryItem inventoryItem)
    {
        var equipmentPlacements = GetAll();

        var indexToDelete = equipmentPlacements.FindIndex(e => e.Equals(inventoryItem));
        if (indexToDelete == -1) throw new KeyNotFoundException();

        equipmentPlacements.RemoveAt(indexToDelete);

        CsvSerializer<InventoryItem>.ToCSV(equipmentPlacements, FilePath);
    }

    public void DeleteAll()
    {
        var equipmentPlacements = _equipmentPlacements ?? GetAll();
        equipmentPlacements.Clear();
        CsvSerializer<InventoryItem>.ToCSV(equipmentPlacements, FilePath);
        _equipmentPlacements = null;
    }
}