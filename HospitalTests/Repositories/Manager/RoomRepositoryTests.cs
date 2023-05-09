using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class RoomRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        const string roomFilePath = "../../../Data/rooms.csv";
        if (File.Exists(roomFilePath))
            File.Delete(roomFilePath);

        RoomRepository.Instance.DeleteAll();
        EquipmentPlacementRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();
    }

    [TestMethod]
    public void TestGetAll()
    {
        EquipmentPlacementRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();

        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom),
            new("2", "Operating room", Room.RoomType.OperatingRoom),
            new("3", "Hybrid operating room", Room.RoomType.OperatingRoom),
            new("4", "Examination room", Room.RoomType.ExaminationRoom),
            new("5", "Ward", Room.RoomType.Ward),
            new("6", "Intensive care unit", Room.RoomType.Ward)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var loadedRooms = RoomRepository.Instance.GetAll();
        Assert.AreEqual(rooms.Count, loadedRooms.Count);
        ;
    }

    [TestMethod]
    public void TestGetById()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom),
            new("2", "Operating room", Room.RoomType.OperatingRoom),
            new("4", "Examination room", Room.RoomType.ExaminationRoom),
            new("5", "Ward", Room.RoomType.Ward)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var foundRoom = RoomRepository.Instance.GetById("2");
        Assert.IsNotNull(foundRoom);
        Assert.AreEqual("Operating room", foundRoom.Name);
        Assert.AreEqual(Room.RoomType.OperatingRoom, foundRoom.Type);
    }

    [TestMethod]
    public void TestGetAllNonExistentFile()
    {
        if (File.Exists("../../../Data/rooms.csv")) File.Delete("../../../Data/rooms.csv");

        Assert.AreEqual(0, RoomRepository.Instance.GetAll().Count);
    }


    [TestMethod]
    public void TestGetByIdNonExistentId()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom),
            new("2", "Operating room", Room.RoomType.OperatingRoom),
            new("4", "Examination room", Room.RoomType.ExaminationRoom),
            new("5", "Ward", Room.RoomType.Ward)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var foundRoom = RoomRepository.Instance.GetById("");
        Assert.IsNull(foundRoom);
    }

    [TestMethod]
    public void TestAdd()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom),
            new("2", "Operating room", Room.RoomType.OperatingRoom),
            new("4", "Examination room", Room.RoomType.ExaminationRoom),
            new("5", "Ward", Room.RoomType.Ward)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var roomRepository = RoomRepository.Instance;
        roomRepository.Add(new Room("7", "New room", Room.RoomType.WaitingRoom));

        Assert.AreEqual(rooms.Count + 1, roomRepository.GetAll().Count);
        Assert.IsNotNull(roomRepository.GetById("7"));
    }

    [TestMethod]
    public void TestUpdate()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var roomRepository = RoomRepository.Instance;
        var idToUpdate = "1";
        var newName = "Examination room";
        var newType = Room.RoomType.ExaminationRoom;
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
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");
        var roomRepository = RoomRepository.Instance;

        roomRepository.Delete(rooms[1]);

        Assert.AreEqual(1, roomRepository.GetAll().Count);
        Assert.IsNull(roomRepository.GetById(rooms[1].Id));
    }

    [TestMethod]
    public void TestGetAllEquipmentLoading()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", Room.RoomType.Warehouse),
            new("1", "Waiting room", Room.RoomType.WaitingRoom)
        };
        Serializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        EquipmentPlacementRepository.Instance.DeleteAll();
        var equipmentInRooms = new List<EquipmentPlacement>
        {
            new("1", "0", 1),
            new("2", "0", 2),
            new("1", "1", 3)
        };
        Serializer<EquipmentPlacement>.ToCSV(equipmentInRooms, "../../../Data/equipmentItems.csv");

        var loadedRooms = RoomRepository.Instance.GetAll();

        Assert.AreEqual(2, loadedRooms[0].Equipment.Count);
        Assert.AreEqual(1, loadedRooms[1].Equipment.Count);
        Assert.AreEqual(1,
            loadedRooms[0].GetAmount(new Equipment("1", "", Equipment.EquipmentType.ExaminationEquipment)));
        Assert.AreEqual(3,
            loadedRooms[1].GetAmount(new Equipment("1", "", Equipment.EquipmentType.ExaminationEquipment)));
    }
}