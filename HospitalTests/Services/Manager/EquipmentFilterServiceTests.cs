using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace HospitalTests.Services.Manager;

[TestClass]
public class EquipmentFilterServiceTests
{
    [TestInitialize]
    public void SetUp()
    {
        EquipmentRepository.Instance.DeleteAll();
        var filesUsed = new List<string>()
        {
            "../../../Data/equipment.csv",
            "../../../Data/equipmentItems.csv",
            "../../../Data/rooms.csv"
        };
        foreach (var file in filesUsed) if (File.Exists(file)) File.Delete(file);


        var equipmentRepository = EquipmentRepository.Instance;
        var equipment = new List<Equipment>
        {
            new("1001", "Examination Table", Equipment.EquipmentType.ExaminationEquipment),
            new("1002", "MRI Machine", Equipment.EquipmentType.ExaminationEquipment),
            new("1003", "Stretcher", Equipment.EquipmentType.HallwayEquipment),
            new("1004", "Surgery Table", Equipment.EquipmentType.OperationEquipment),
            new("1005", "Wheelchair", Equipment.EquipmentType.HallwayEquipment),
            new("1006", "X-ray Machine", Equipment.EquipmentType.ExaminationEquipment),
            new("1007", "Oxygen Concentrator", Equipment.EquipmentType.Furniture),
            new("1008", "EKG Machine", Equipment.EquipmentType.ExaminationEquipment),
            new("1009", "Operating Room Lights", Equipment.EquipmentType.OperationEquipment),
            new("1010", "Hospital Bed", Equipment.EquipmentType.Furniture),
            new("1011", "Blood Pressure Monitor", Equipment.EquipmentType.ExaminationEquipment),
            new("1012", "Defibrillator", Equipment.EquipmentType.OperationEquipment),
            new("1013", "Ultrasound Machine", Equipment.EquipmentType.ExaminationEquipment),
            new("1014", "Sterilization Machine", Equipment.EquipmentType.OperationEquipment),
            new("1015", "Gurney", Equipment.EquipmentType.HallwayEquipment),
            new("1016", "Anesthesia Machine", Equipment.EquipmentType.OperationEquipment),
            new("1017", "Nebulizer", Equipment.EquipmentType.ExaminationEquipment),
            new("1018", "Crash Cart", Equipment.EquipmentType.OperationEquipment),
            new("1019", "Medical Refrigerator", Equipment.EquipmentType.Furniture),
            new("1020", "Oxygen Tank", Equipment.EquipmentType.HallwayEquipment),
            new("1021", "Chair", Equipment.EquipmentType.Furniture),
            new("1022", "Magazine stand", Equipment.EquipmentType.Furniture)


        };

        foreach (var e in equipment) equipmentRepository.Add(e);

        var roomRepository = new RoomRepository();

        var rooms = new List<Room>()
        {

            new Room("5000","Warehouse Room", Room.RoomType.Warehouse),
            new Room("0001","Operating Room 1", Room.RoomType.OperatingRoom),
            new Room("0002","Operating Room 2", Room.RoomType.OperatingRoom),

            new Room("1001","Examination Room 1", Room.RoomType.ExaminationRoom),

            new Room("1002", "Examination Room 2", Room.RoomType.ExaminationRoom),

            new Room("1003","Examination Room 3", Room.RoomType.ExaminationRoom),

            new Room("1004", "Examination Room 4", Room.RoomType.ExaminationRoom),
            new Room("2001","Waiting Room 1", Room.RoomType.WaitingRoom),


            new Room("2002","Waiting Room 2", Room.RoomType.WaitingRoom),
            new Room("3001","Ward Room 101", Room.RoomType.Ward),

            new Room("3002","Ward Room 102", Room.RoomType.Ward),

            new Room("3003", "Ward Room 103", Room.RoomType.Ward),

            new Room("3004","Ward Room 104", Room.RoomType.Ward)
        };

        foreach (var room in rooms)
        {
            roomRepository.Add(room);
        }

        var equipmentPlacements = new List<EquipmentPlacement>()
        {
            new EquipmentPlacement("1021", "5000", 15), // 15 chairs in warehouse
            new EquipmentPlacement("1021", "2001", 3), // 3 Chairs in Waiting Room 1
            new EquipmentPlacement("1022", "2001", 1), // 1 Magazine stand in Waiting room 2
            new EquipmentPlacement("1021", "2002", 2), // 2 Chairs in Waiting Room 2
            // 2 Chairs in every examination room
            new EquipmentPlacement("1021", "1001", 2),
            new EquipmentPlacement("1021", "1002", 2),
            new EquipmentPlacement("1021", "1003", 2),
            new EquipmentPlacement("1021", "1004", 2),

            new EquipmentPlacement("1008", "1001", 1), // 1 EKG machine in examination room 1
            new EquipmentPlacement("1006", "1002", 1), // 1 Xray machine in examination room 2
            new EquipmentPlacement("1011", "1003", 2), // 2 blood pressure monitors in examination room 3
            new EquipmentPlacement("1011", "1003", 1), // 1 blood pressure monitor in examination room 3
            new EquipmentPlacement("1004", "1004", 1), // 1 nebulizer in examination room 4

            new EquipmentPlacement("1009", "0001", 1), // 1 Operating room lights in operating room 1
            new EquipmentPlacement("1004", "0001", 1), // 1 Operating table in operating room 1
            new EquipmentPlacement("1018", "0001", 1), // 1 Crash cart in operating room 1

            new EquipmentPlacement("1005", "5000", 3), // 3 Wheelchairs in Warehouse
            new EquipmentPlacement("1011", "5000", 7), // 7 Blood pressure monitors in Warehouse
            new EquipmentPlacement("1007", "5000", 2), // 2 Oxygen tanks in Warehouse
            new EquipmentPlacement("1010", "5000", 11), // 11 Beds in Warehouse




        };

        // 3 beds in every ward
        foreach (var ward in rooms.Where(room => room.Type == Room.RoomType.Ward))
        {
            equipmentPlacements.Add(new EquipmentPlacement("1010", ward.Id, 3));
        }

        //equipmentPlacements.Add(new EquipmentPlacement("1010", "3004", 3));


        var equipmentPlacementRepository = new EquipmentPlacementRepository();
        foreach (var equipmentPlacement in equipmentPlacements)
        {
            equipmentPlacementRepository.Add(equipmentPlacement);
        }
    }

    [TestMethod]
    public void TestGetEquipmentInRoomType()
    {
        var equipmentFilterService = new EquipmentFilterService();

        var equipmentInExaminationRooms =
            equipmentFilterService.GetEquipment(Room.RoomType.ExaminationRoom);
        var equipmentInWaitingRooms = equipmentFilterService.GetEquipment(Room.RoomType.WaitingRoom);

        Assert.AreEqual(2, equipmentInWaitingRooms.Count); // magazine stand and chairs
        Assert.AreEqual(5, equipmentInExaminationRooms.Count); // EKG, Xray, Blood pressure monitor, chair, nebulizer
    }

    [TestMethod]
    public void TestGetEquipmentOfType()
    {
        var equipmentFilterService = new EquipmentFilterService();
        Assert.AreEqual(7, equipmentFilterService.GetEquipment(Equipment.EquipmentType.ExaminationEquipment).Count);
        Assert.AreEqual(5, equipmentFilterService.GetEquipment(Equipment.EquipmentType.Furniture).Count);
    }

    [TestMethod]
    public void TestGetEquipmentByAmount()
    {
        var equipmentFilterService = new EquipmentFilterService();

        Assert.AreEqual(22, equipmentFilterService.GetEquipment(30).Count);
        Assert.AreEqual(20, equipmentFilterService.GetEquipment(22).Count); // There are 23 chairs and beds each in the hospital and only they won't be returned.
        Assert.AreEqual(11, equipmentFilterService.GetEquipment(0).Count);
    }

    [TestMethod]
    public void TestSelectEquipment()
    {
        var equipmentFilterService = new EquipmentFilterService();
        var roomRepository = new RoomRepository();

        var warehouse = roomRepository.GetById("5000");

        Assert.IsNotNull(warehouse);
        Assert.AreEqual(3, equipmentFilterService.Select(warehouse.Equipment, 10).Count);

    }

    [TestMethod]
    public void TestGetEquipmentNotInWarehouse()
    {
        var equipmentFilterService = new EquipmentFilterService();
        var equipmentNotInWarehouse = equipmentFilterService.GetEquipmentNotInWarehouse();
        Assert.AreEqual(22 - 5, equipmentNotInWarehouse.Count);

    }

    [TestMethod]
    public void TestSelectByStringTypeOnly()
    {
        var allEquipment = EquipmentRepository.Instance.GetAll();
        var equipmentFilterService = new EquipmentFilterService();
        var furniture = equipmentFilterService.Select(allEquipment, "fUrNiture");
        
        Assert.AreEqual(5, furniture.Count);

    }

    [TestMethod]
    public void TestSelectByStringBothTypeAndName()
    {
        var allEquipment = EquipmentRepository.Instance.GetAll();
        var equipmentFilterService = new EquipmentFilterService();
        var equipmentWithF = equipmentFilterService.Select(allEquipment, "f");

        Assert.AreEqual(6, equipmentWithF.Count); // Should select 5 furniture and defibrilator
    }
}