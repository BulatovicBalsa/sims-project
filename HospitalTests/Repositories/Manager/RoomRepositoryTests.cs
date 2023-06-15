﻿using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
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
        InventoryItemRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();
    }

    [TestMethod]
    public void TestGetAll()
    {
        InventoryItemRepository.Instance.DeleteAll();
        EquipmentRepository.Instance.DeleteAll();

        var rooms = new List<Room>
        {
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom),
            new("2", "Operating room", RoomType.OperatingRoom),
            new("3", "Hybrid operating room", RoomType.OperatingRoom),
            new("4", "Examination room", RoomType.ExaminationRoom),
            new("5", "Ward", RoomType.Ward),
            new("6", "Intensive care unit", RoomType.Ward)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var loadedRooms = RoomRepository.Instance.GetAll();
        Assert.AreEqual(rooms.Count, loadedRooms.Count);
        ;
    }

    [TestMethod]
    public void TestGetById()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom),
            new("2", "Operating room", RoomType.OperatingRoom),
            new("4", "Examination room", RoomType.ExaminationRoom),
            new("5", "Ward", RoomType.Ward)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var foundRoom = RoomRepository.Instance.GetById("2");
        Assert.IsNotNull(foundRoom);
        Assert.AreEqual("Operating room", foundRoom.Name);
        Assert.AreEqual(RoomType.OperatingRoom, foundRoom.Type);
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
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom),
            new("2", "Operating room", RoomType.OperatingRoom),
            new("4", "Examination room", RoomType.ExaminationRoom),
            new("5", "Ward", RoomType.Ward)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var foundRoom = RoomRepository.Instance.GetById("");
        Assert.IsNull(foundRoom);
    }

    [TestMethod]
    public void TestAdd()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom),
            new("2", "Operating room", RoomType.OperatingRoom),
            new("4", "Examination room", RoomType.ExaminationRoom),
            new("5", "Ward", RoomType.Ward)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var roomRepository = RoomRepository.Instance;
        roomRepository.Add(new Room("7", "New room", RoomType.WaitingRoom));

        Assert.AreEqual(rooms.Count + 1, roomRepository.GetAll().Count);
        Assert.IsNotNull(roomRepository.GetById("7"));
    }

    [TestMethod]
    public void TestUpdate()
    {
        var rooms = new List<Room>
        {
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        var roomRepository = RoomRepository.Instance;
        var idToUpdate = "1";
        var newName = "Examination room";
        var newType = RoomType.ExaminationRoom;
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
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");
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
            new("0", "Warehouse", RoomType.Warehouse),
            new("1", "Waiting room", RoomType.WaitingRoom)
        };
        CsvSerializer<Room>.ToCSV(rooms, "../../../Data/rooms.csv");

        InventoryItemRepository.Instance.DeleteAll();
        var equipmentInRooms = new List<InventoryItem>
        {
            new("1", "0", 1),
            new("2", "0", 2),
            new("1", "1", 3)
        };
        CsvSerializer<InventoryItem>.ToCSV(equipmentInRooms, "../../../Data/equipmentItems.csv");

        var loadedRooms = RoomRepository.Instance.GetAll();

        Assert.AreEqual(2, loadedRooms[0].Inventory.Count);
        Assert.AreEqual(1, loadedRooms[1].Inventory.Count);
        Assert.AreEqual(1,
            loadedRooms[0].GetAmount(new Equipment("1", "", EquipmentType.ExaminationEquipment)));
        Assert.AreEqual(3,
            loadedRooms[1].GetAmount(new Equipment("1", "", EquipmentType.ExaminationEquipment)));
    }
}