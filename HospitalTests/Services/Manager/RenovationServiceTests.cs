using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.Core.PhysicalAssets.Services;
using Hospital.Core.Scheduling.Services;

namespace HospitalTests.Services.Manager;

[TestClass]
public class RenovationServiceTests
{
    private const string renovationFilePath = "../../../Data/renovations.csv";

    [TestInitialize]
    public void SetUp()
    {
        if (File.Exists(renovationFilePath)) File.Delete(renovationFilePath);
        RenovationRepository.Instance.GetAllFromFile();
    }

    [TestCleanup]
    public void CleanUp()
    {
        File.Delete(renovationFilePath);
    }

    [TestMethod]
    public void TestAddRenovationRoomHasExamination()
    {
        var room = new Room();
        var examination = new Examination(null, new Patient(), true, DateTime.Now, room);
        var examinations = new List<Examination>
        {
            examination
        };
        var renovations = new List<Renovation>
        {
            new("", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-3), room)
        };
        var roomScheduleService = new RoomScheduleService(examinations, renovations);
        var renovationService = new RenovationService(roomScheduleService, null);
        Assert.IsFalse(
            renovationService.AddRenovation(new Renovation(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), room)));
    }

    [TestMethod]
    public void TestAddRenovationRoomHasRenovation()
    {
        var room = new Room();
        var examination = new Examination(null, new Patient(), true, DateTime.Now, room);
        var examinations = new List<Examination>
        {
            examination
        };
        var renovations = new List<Renovation>
        {
            new("", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-3), room)
        };
        var roomScheduleService = new RoomScheduleService(examinations, renovations);
        var renovationService = new RenovationService(roomScheduleService, null);
        Assert.IsFalse(
            renovationService.AddRenovation(new Renovation(DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-3), room)));
    }

    [TestMethod]
    public void TestAddRenovationSuccessful()
    {
        var roomScheduleService = new RoomScheduleService(new List<Examination>(), new List<Renovation>());
        var renovationService = new RenovationService(roomScheduleService);
        Assert.IsTrue(renovationService.AddRenovation(new Renovation("1", DateTime.Now, DateTime.Now.AddDays(1),
            new Room("1", "Ward", RoomType.Ward))));
    }

    [TestMethod]
    public void TestTryCompleteAllRenovations()
    {
        var roomScheduleService = new RoomScheduleService(new List<Examination>(), new List<Renovation>());
        var renovationService = new RenovationService(roomScheduleService, RenovationRepository.Instance);
        Assert.IsTrue(renovationService.AddRenovation(new Renovation("1", DateTime.Now.AddDays(-3),
            DateTime.Now.AddDays(-1),
            new Room("1", "Ward", RoomType.Ward))));
        Assert.IsTrue(renovationService.AddRenovation(new Renovation("1", DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(3),
            new Room("1", "Ward", RoomType.Ward))));

        Assert.AreEqual(2, RenovationRepository.Instance.GetAll().Count);
        renovationService.TryCompleteAllRenovations();
        Assert.IsTrue(RenovationRepository.Instance.GetAll()[0].Completed);
        Assert.IsFalse(RenovationRepository.Instance.GetAll()[1].Completed);
    }
}