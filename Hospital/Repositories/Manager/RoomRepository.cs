﻿using System.Collections.Generic;
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
            _rooms = Serializer<Room>.FromCSV(FilePath);
            PlaceEquipment(_rooms);
        }

        return _rooms;
    }

    private static void PlaceEquipment(List<Room> rooms)
    {
        var equipmentPlacements = InventoryItemRepository.Instance.GetAll();

        var equipmentPlacementsByRoom =
            from equipmentPlacement in equipmentPlacements
            group equipmentPlacement by equipmentPlacement.RoomId
            into equipmentInRoom
            select equipmentInRoom;

        foreach (var roomGroup in equipmentPlacementsByRoom)
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

    public void Add(Room room)
    {
        var rooms = GetAll();

        rooms.Add(room);

        Serializer<Room>.ToCSV(rooms, FilePath);

        foreach (var equipmentPlacement in room.Inventory)
            InventoryItemRepository.Instance.Add(equipmentPlacement);
    }

    public void Update(Room room)
    {
        var rooms = GetAll();

        var indexToUpdate = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        rooms[indexToUpdate] = room;

        Serializer<Room>.ToCSV(rooms, FilePath);

        foreach (var equipmentPlacement in room.Inventory)
            if (InventoryItemRepository.Instance.GetByKey(equipmentPlacement.RoomId,
                    equipmentPlacement.EquipmentId) == null)
                InventoryItemRepository.Instance.Add(equipmentPlacement);
            else
                InventoryItemRepository.Instance.Update(equipmentPlacement);
    }

    public void Delete(Room room)
    {
        var rooms = GetAll();

        var indexToDelete = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        rooms.RemoveAt(indexToDelete);

        Serializer<Room>.ToCSV(rooms, FilePath);
    }

    public void DeleteAll()
    {
        var rooms = _rooms ?? GetAll();
        rooms.Clear();
        Serializer<Room>.ToCSV(rooms, FilePath);
        _rooms = null;
    }

    public Room GetWarehouse()
    {
        var warehouse = GetAll().Find(room => room.Type == RoomType.Warehouse);
        return warehouse ?? throw new NoWarehouseException();
    }
}