using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;
using Hospital.Services.Manager;

namespace HospitalTests.Services.Manager;

[TestClass]
public class ComplexRenovationServiceTests
{
    [TestInitialize]
    public void SetUp()
    {
        DeleteData();
        RenovationRepository.Instance.GetAllFromFile();
    }

    [TestCleanup]
    public void CleanUp()
    {
        DeleteData();
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
        var complexRenovation = new ComplexRenovation(new List<Room>() { ward, examRoom },
            new List<Room>() { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom, new List<Transfer>());
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
        var complexRenovation = new ComplexRenovation(new List<Room>() { ward, examRoom },
            new List<Room>() { operatingRoom }, new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)), operatingRoom, new List<Transfer>());
        complexRenovationService.AddComplexRenovation(complexRenovation);
    
        var otherComplexRenovation = new ComplexRenovation(new List<Room>() { ward, examRoom },
            new List<Room>() { operatingRoom }, new TimeRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5)), operatingRoom, new List<Transfer>());

        Assert.IsFalse(complexRenovationService.CanBePerformed(otherComplexRenovation));
    }
}