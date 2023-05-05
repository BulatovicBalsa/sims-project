using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class RoomRepository
{
    public const string FilePath = "../../../Data/rooms.csv";
    private static RoomRepository? _instance;
    private List<Room>? _rooms = null;

    public static RoomRepository Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomRepository();
            }

            return _instance;
        }
    }

    private RoomRepository()
    {

    }

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
        var equipmentPlacements = EquipmentPlacementRepository.Instance.GetAll();

        var equipmentPlacementsByRoom =
            from equipmentPlacement in equipmentPlacements
            group equipmentPlacement by equipmentPlacement.RoomId
            into equipmentInRoom
            select equipmentInRoom;

        foreach (var roomGroup in equipmentPlacementsByRoom)
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

        foreach (var equipmentPlacement in room.Equipment)
        {
            EquipmentPlacementRepository.Instance.Add(equipmentPlacement);
        }
    }

    public void Update(Room room)
    {
        var rooms = GetAll();

        var indexToUpdate = rooms.FindIndex(e => e.Id == room.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        rooms[indexToUpdate] = room;

        Serializer<Room>.ToCSV(rooms, FilePath);

        foreach (var equipmentPlacement in room.Equipment)
        {
            if (EquipmentPlacementRepository.Instance.GetByKey(equipmentPlacement.RoomId,
                    equipmentPlacement.EquipmentId) == null)
            {
                EquipmentPlacementRepository.Instance.Add(equipmentPlacement);
            }
            else
            {
                EquipmentPlacementRepository.Instance.Update(equipmentPlacement);
            }
        }
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
        if (_rooms == null) return;
        _rooms.Clear();
        Serializer<Room>.ToCSV(_rooms, FilePath);
        _rooms = null;
    }
}