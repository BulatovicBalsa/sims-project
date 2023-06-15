using Hospital.PatientHealthcare.Models;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
using Hospital.PhysicalAssets.Services;
using Hospital.Scheduling;
using Hospital.Scheduling.Services;

namespace HospitalTests.Services.Manager;

[TestClass]
public class ComplexRenovationServiceTests
{
    [TestInitialize]
    public void SetUp()
    {
        try
        {
            DeleteData();
        }
        catch (Exception)
        {
            Console.WriteLine("Files don't exist.");
        }

        RenovationRepository.Instance.GetAllFromFile();
    }

    [TestCleanup]
    public void CleanUp()
    {
        try
        {
            DeleteData();
        }
        catch (Exception)
        {
            Console.WriteLine("Files don't exist.");
        }
    }

    private static void DeleteData()
    {
        Directory.GetFiles("../../../Data/").ToList().ForEach(File.Delete);
    }

    [TestMethod]
    public void TestAddComplexRenovation()
    {
        Assert.AreEqual(0, RenovationRepository.Instance.GetAll().Count);
        var warehouse = new Room("Warehouse", RoomType.Warehouse);
        var examRoom = new Room("Examination room", RoomType.ExaminationRoom);
        var ward = new Room("Ward", RoomType.Ward);
        var rooms = new List<Room>
        {
            warehouse,
            ward,
            examRoom
        };
        var operatingRoom = new Room("Operating room", RoomType.OperatingRoom);
        RoomRepository.Instance.Add(rooms);

        var roomScheduleService = new RoomScheduleService();

        var complexRenovationService = new ComplexRenovationService(roomScheduleService);
        var complexRenovation = new ComplexRenovation(new List<Room> { ward, examRoom },
            new List<Room> { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom,
            new List<Transfer>());
        complexRenovationService.AddComplexRenovation(complexRenovation);

        Assert.AreEqual(3, RenovationRepository.Instance.GetAll().Count);
    }

    [TestMethod]
    public void TestCanPerformComplexRenovationTwice()
    {
        Assert.AreEqual(0, RenovationRepository.Instance.GetAll().Count);
        var warehouse = new Room("Warehouse", RoomType.Warehouse);
        var examRoom = new Room("Examination room", RoomType.ExaminationRoom);
        var ward = new Room("Ward", RoomType.Ward);
        var rooms = new List<Room>
        {
            warehouse,
            ward,
            examRoom
        };
        var operatingRoom = new Room("Operating room", RoomType.OperatingRoom);
        RoomRepository.Instance.Add(rooms);

        var roomScheduleService = new RoomScheduleService();

        var complexRenovationService = new ComplexRenovationService(roomScheduleService);
        var complexRenovation = new ComplexRenovation(new List<Room> { ward, examRoom },
            new List<Room> { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom,
            new List<Transfer>());
        complexRenovationService.AddComplexRenovation(complexRenovation);

        var otherComplexRenovation = new ComplexRenovation(new List<Room> { ward, examRoom },
            new List<Room> { operatingRoom }, new TimeRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5)),
            operatingRoom, new List<Transfer>());

        Assert.IsFalse(complexRenovationService.CanBePerformed(otherComplexRenovation));
    }

    [TestMethod]
    public void TestCanNotPerformComplexRenovationBecauseOfExaminations()
    {
        Assert.AreEqual(0, RenovationRepository.Instance.GetAll().Count);
        var warehouse = new Room("Warehouse", RoomType.Warehouse);
        var examRoom = new Room("Examination room", RoomType.ExaminationRoom);
        var ward = new Room("Ward", RoomType.Ward);
        var rooms = new List<Room>
        {
            warehouse,
            ward,
            examRoom
        };
        var operatingRoom = new Room("Operating room", RoomType.OperatingRoom);
        RoomRepository.Instance.Add(rooms);

        var examinations = new List<Examination>
        {
            new(null, new Patient(), false, DateTime.Now.AddDays(20), examRoom)
        };

        var roomScheduleService = new RoomScheduleService(examinations, new List<Renovation>());

        var complexRenovationService = new ComplexRenovationService(roomScheduleService);
        var complexRenovation = new ComplexRenovation(new List<Room> { ward, examRoom },
            new List<Room> { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom,
            new List<Transfer>());

        Assert.IsFalse(complexRenovationService.CanBePerformed(complexRenovation));
    }

    [TestMethod]
    public void TestCanNotPerformComplexRenovationBecauseOfOtherRenovations()
    {
        Assert.AreEqual(0, RenovationRepository.Instance.GetAll().Count);
        var warehouse = new Room("Warehouse", RoomType.Warehouse);
        var examRoom = new Room("Examination room", RoomType.ExaminationRoom);
        var ward = new Room("Ward", RoomType.Ward);
        var rooms = new List<Room>
        {
            warehouse,
            ward,
            examRoom
        };
        var operatingRoom = new Room("Operating room", RoomType.OperatingRoom);
        RoomRepository.Instance.Add(rooms);

        var renovations = new List<Renovation>
        {
            new("", DateTime.Now.AddDays(20), DateTime.Now.AddDays(30), ward)
        };
        ;

        var roomScheduleService = new RoomScheduleService(new List<Examination>(), renovations);

        var complexRenovationService = new ComplexRenovationService(roomScheduleService);
        var complexRenovation = new ComplexRenovation(new List<Room> { ward, examRoom },
            new List<Room> { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom,
            new List<Transfer>());

        Assert.IsFalse(complexRenovationService.CanBePerformed(complexRenovation));
    }

    [TestMethod]
    public void TestCanNotPerformComplexRenovationBecauseOfTransfers()
    {
        Assert.AreEqual(0, RenovationRepository.Instance.GetAll().Count);
        var warehouse = new Room("Warehouse", RoomType.Warehouse);
        var examRoom = new Room("Examination room", RoomType.ExaminationRoom);
        var ward = new Room("Ward", RoomType.Ward);
        var rooms = new List<Room>
        {
            warehouse,
            ward,
            examRoom
        };
        var operatingRoom = new Room("Operating room", RoomType.OperatingRoom);
        RoomRepository.Instance.Add(rooms);

        var transfers = new List<Transfer>
        {
            new(ward, examRoom, DateTime.Now.AddDays(5))
        };
        var roomScheduleService = new RoomScheduleService(new List<Examination>(), new List<Renovation>(), transfers);

        var complexRenovationService = new ComplexRenovationService(roomScheduleService);
        var complexRenovation = new ComplexRenovation(new List<Room> { ward, examRoom },
            new List<Room> { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom,
            new List<Transfer>());

        Assert.IsFalse(complexRenovationService.CanBePerformed(complexRenovation));
    }
}