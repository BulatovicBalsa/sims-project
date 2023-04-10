using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class RoomRepository
{
    public const string FilePath = "../../../Data/rooms.csv";

    public List<Room> GetAll()
    {
        var rooms = Serializer<Room>.FromCSV(FilePath);
        PlaceEquipment(rooms);

        return rooms;
    }

    private static void PlaceEquipment(List<Room> rooms)
    {
        var equipmentItems = new EquipmentItemRepository().GetAll();

        var equipmentItemsByRoom =
            from equipmentItem in equipmentItems
            group equipmentItem by equipmentItem.RoomId
            into equipmentInRoom
            select equipmentInRoom;

        foreach (var roomGroup in equipmentItemsByRoom)
        {
            var room = rooms.Find(room => room.Id == roomGroup.Key);
            if (room == null) continue;
            room.Equipment = roomGroup.ToList();
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
    }

    public void Update(Room room)
    {
        var rooms = GetAll();

        var indexToUpdate = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        rooms[indexToUpdate] = room;

        Serializer<Room>.ToCSV(rooms, FilePath);
    }

    public void Delete(Room room)
    {
        var rooms = GetAll();

        var indexToDelete = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        rooms.RemoveAt(indexToDelete);

        Serializer<Room>.ToCSV(rooms, FilePath);
    }
}