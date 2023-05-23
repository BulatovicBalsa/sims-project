using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Services.Manager;

namespace HospitalTests.Services.Manager;

[TestClass]
public class RenovationServiceTests
{
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
        Assert.IsFalse(renovationService.AddRenovation(new Renovation(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), room)));
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
        Assert.IsFalse(renovationService.AddRenovation(new Renovation(DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-3), room)));
    }

    [TestMethod]
    public void TestAddRenovationSuccessful()
    {
        var roomScheduleService = new RoomScheduleService(new List<Examination>(), new List<Renovation>());
        var renovationService = new RenovationService(roomScheduleService);
        Assert.IsTrue(renovationService.AddRenovation(new Renovation("1", DateTime.Now, DateTime.Now.AddDays(1),
            new Room("1", "Ward", RoomType.Ward))));
    }
}