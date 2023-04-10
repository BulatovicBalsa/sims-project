using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class RoomRepositoryTests
{
    [TestMethod]
    public void TestGetAll()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM),
            new("2", "Operating room", Room.RoomType.OPERATING_ROOM),
            new("3", "Hybrid operating room", Room.RoomType.OPERATING_ROOM),
            new("4", "Examination room", Room.RoomType.EXAMINATION_ROOM),
            new("5", "Ward", Room.RoomType.WARD),
            new("6", "Intensive care unit", Room.RoomType.WARD)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var loadedRooms = new RoomRepository().GetAll();
        Assert.AreEqual(rooms.Count, loadedRooms.Count);
        ;
    }

    [TestMethod]
    public void TestGetById()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM),
            new("2", "Operating room", Room.RoomType.OPERATING_ROOM),
            new("4", "Examination room", Room.RoomType.EXAMINATION_ROOM),
            new("5", "Ward", Room.RoomType.WARD)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var foundRoom = new RoomRepository().GetById("2");
        Assert.IsNotNull(foundRoom);
        Assert.AreEqual("Operating room", foundRoom.Name);
        Assert.AreEqual(Room.RoomType.OPERATING_ROOM, foundRoom.Type);
    }

    [TestMethod]
    public void TestGetAllNonExistentFile()
    {
        if (File.Exists("../../../Data/rooms.csv")) File.Delete("../../../Data/rooms.csv");

        Assert.AreEqual(0, new RoomRepository().GetAll().Count);
    }


    [TestMethod]
    public void TestGetByIdNonExistentId()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM),
            new("2", "Operating room", Room.RoomType.OPERATING_ROOM),
            new("4", "Examination room", Room.RoomType.EXAMINATION_ROOM),
            new("5", "Ward", Room.RoomType.WARD)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var foundRoom = new RoomRepository().GetById("");
        Assert.IsNull(foundRoom);
    }

    [TestMethod]
    public void TestAdd()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM),
            new("2", "Operating room", Room.RoomType.OPERATING_ROOM),
            new("4", "Examination room", Room.RoomType.EXAMINATION_ROOM),
            new("5", "Ward", Room.RoomType.WARD)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var roomRepository = new RoomRepository();
        roomRepository.Add(new Room("7", "New room", Room.RoomType.WAITING_ROOM));

        Assert.AreEqual(rooms.Count + 1, roomRepository.GetAll().Count);
        Assert.IsNotNull(roomRepository.GetById("7"));
    }

    [TestMethod]
    public void TestUpdate()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var roomRepository = new RoomRepository();
        var idToUpdate = "1";
        var newName = "Examination room";
        var newType = Room.RoomType.EXAMINATION_ROOM;
        roomRepository.Update(new Room(idToUpdate, newName, newType));

        var updatedRoom = roomRepository.GetById(idToUpdate);

        Assert.IsNotNull(updatedRoom);
        Assert.AreEqual(newName, updatedRoom.Name);
        Assert.AreEqual(newType, updatedRoom.Type);
    }

    [TestMethod]
    public void TestDelete()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");
        var roomRepository = new RoomRepository();

        roomRepository.Delete(rooms[1]);

        Assert.AreEqual(1, roomRepository.GetAll().Count);
        Assert.IsNull(roomRepository.GetById(rooms[1].Id));
    }

    [TestMethod]
    public void TestGetAllEquipmentLoading()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.WAREHOUSE),
            new("1", "Waiting room", Room.RoomType.WAITING_ROOM)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var equipmentInRooms = new List<EquipmentItem>()
        {
            new("1", "0", 1),
            new("2", "0", 2),
            new("1", "1", 3)
        };
        Serializer<EquipmentItem>.ToCSV(equipmentInRooms, "../../../Data/equipmentItems.csv");

        var loadedRooms = new RoomRepository().GetAll();

        Assert.AreEqual(2, loadedRooms[0].Equipment.Count);
        Assert.AreEqual(1, loadedRooms[1].Equipment.Count);
        Assert.AreEqual(1, loadedRooms[0].GetAmount(new Equipment("1", "", Equipment.EquipmentType.EXAMINATION_EQUIPMENT)));
        Assert.AreEqual(3, loadedRooms[1].GetAmount(new Equipment("1", "", Equipment.EquipmentType.EXAMINATION_EQUIPMENT)));
        

    }
}