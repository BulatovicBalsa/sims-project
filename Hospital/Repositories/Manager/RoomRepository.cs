using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Exceptions;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class RoomRepository
{
    public const string FilePath = "../../../Data/rooms.csv";
    private static RoomRepository? _instance;
    private List<Room>? _rooms;

    private RoomRepository()
    {
    }

    public static RoomRepository Instance => _instance ??= new RoomRepository();

    public List<Room> GetAll()
    {
        if (_rooms == null)
        {
            _rooms = CsvSerializer<Room>.FromCSV(FilePath);
            JoinWithInventories(_rooms);
        }

        return _rooms;
    }

    private static void JoinWithInventories(List<Room> rooms)
    {
        var inventoryItems = InventoryItemRepository.Instance.GetAll();

        var inventoryItemsByRoom =
            from equipmentPlacement in inventoryItems
            group equipmentPlacement by equipmentPlacement.RoomId
            into equipmentInRoom
            select equipmentInRoom;

        foreach (var roomGroup in inventoryItemsByRoom)
        {
            var room = rooms.Find(room => room.Id == roomGroup.Key);
            if (room == null) continue;
            room.Inventory = roomGroup.ToList();
        }
    }

    public Room? GetById(string id)
    {
        return GetAll().Find(equipment => equipment.Id == id);
    }

    public List<Room> Get(List<Room> rooms)
    {
        return rooms.Select(room => GetById(room.Id) ?? throw new InvalidOperationException()).ToList();
    }

    public void Add(Room room)
    {
        var rooms = GetAll();

        rooms.Add(room);

        CsvSerializer<Room>.ToCSV(rooms, FilePath);

        foreach (var inventoryItem in room.Inventory)
            InventoryItemRepository.Instance.Add(inventoryItem);
    }

    public void Add(List<Room> rooms)
    {
        rooms.ForEach(Add);
    }

    public void Update(Room room)
    {
        var rooms = GetAll();

        var indexToUpdate = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        rooms[indexToUpdate] = room;

        CsvSerializer<Room>.ToCSV(rooms, FilePath);

        foreach (var inventoryItem in room.Inventory)
            if (InventoryItemRepository.Instance.GetByKey(inventoryItem.RoomId,
                    inventoryItem.EquipmentId) == null)
                InventoryItemRepository.Instance.Add(inventoryItem);
            else
                InventoryItemRepository.Instance.Update(inventoryItem);
    }

    public void Delete(Room room)
    {
        var rooms = GetAll();

        var indexToDelete = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        rooms.RemoveAt(indexToDelete);

        CsvSerializer<Room>.ToCSV(rooms, FilePath);
    }

    public void DeleteAll()
    {
        var rooms = _rooms ?? GetAll();
        rooms.Clear();
        CsvSerializer<Room>.ToCSV(rooms, FilePath);
        _rooms = null;
    }

    public Room GetWarehouse()
    {
        var warehouse = GetAll().Find(room => room.Type == RoomType.Warehouse);
        return warehouse ?? throw new NoWarehouseException();
    }

    public List<Room> GetAll(RoomType type)
    {
        return GetAll().Where(room => room.Type == type).ToList();
    }
}