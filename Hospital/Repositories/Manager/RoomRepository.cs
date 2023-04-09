using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class RoomRepository
{
    public const string FilePath = "../../../Data/rooms.csv";

    public List<Room> GetAll()
    {
        return Serializer<Room>.FromCSV(FilePath);
    }

    public Room? GetById(string id)
    {
        return GetAll().Find(equipment => equipment.Id == id);
    }

    public void Add(Room equipment)
    {
        var rooms = GetAll();

        rooms.Add(equipment);

        Serializer<Room>.ToCSV(rooms, FilePath);
    }

    public void Update(Room equipment)
    {
        var rooms = GetAll();

        var indexToUpdate = rooms.FindIndex(e => e.Id == equipment.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        rooms[indexToUpdate] = equipment;

        Serializer<Room>.ToCSV(rooms, FilePath);
    }

    public void Delete(Room equipment)
    {
        var rooms = GetAll();

        var indexToDelete = rooms.FindIndex(e => e.Id == equipment.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        rooms.RemoveAt(indexToDelete);

        Serializer<Room>.ToCSV(rooms, FilePath);
    }
}