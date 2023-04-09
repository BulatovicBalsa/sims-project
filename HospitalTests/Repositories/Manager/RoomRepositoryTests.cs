using System.Globalization;
using CsvHelper;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

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
        using (var writer = new StreamWriter("../../../Data/rooms.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(rooms);
            csvWriter.Flush();
        }

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
        using (var writer = new StreamWriter("../../../Data/rooms.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(rooms);
            csvWriter.Flush();
        }

        var foundRoom = new RoomRepository().GetById("2");
        Assert.IsNotNull(foundRoom);
        Assert.AreEqual("Operating room", foundRoom.Name);
        Assert.AreEqual(Room.RoomType.OPERATING_ROOM, foundRoom.Type);
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
        using (var writer = new StreamWriter("../../../Data/rooms.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(rooms);
            csvWriter.Flush();
        }

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
        using (var writer = new StreamWriter("../../../Data/rooms.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(rooms);
            csvWriter.Flush();
        }

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
        using (var writer = new StreamWriter("../../../Data/rooms.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(rooms);
            csvWriter.Flush();
        }

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
        using (var writer = new StreamWriter("../../../Data/rooms.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(rooms);
            csvWriter.Flush();
        }

        var roomRepository = new RoomRepository();

        roomRepository.Delete(rooms[1]);

        Assert.AreEqual(1, roomRepository.GetAll().Count);
        Assert.IsNull(roomRepository.GetById(rooms[1].Id));
    }
}