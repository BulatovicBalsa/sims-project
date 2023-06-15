using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.PatientHealthcare.Models;
using Hospital.PhysicalAssets.Models;
using Hospital.Scheduling;
using Hospital.Scheduling.Services;

namespace HospitalTests.Services.Manager;

[TestClass]
public class RoomScheduleServiceTests
{
    [TestMethod]
    public void TestIsFreeHasExamination()
    {
        var room = new Room();
        var examination = new Examination(null, new Patient(), true, DateTime.Now, room);
        var examinations = new List<Examination>
        {
            examination
        };

        var roomScheduleService = new RoomScheduleService(examinations, new List<Renovation>());

        Assert.IsFalse(roomScheduleService.IsFree(room,
            new TimeRange(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1))));
    }

    [TestMethod]
    public void TestIsFreeHasRenovation()
    {
        var room = new Room();
        var examinations = new List<Examination>();
        var renovations = new List<Renovation>
        {
            new("", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), room)
        };
        var roomScheduleService = new RoomScheduleService(examinations, renovations);

        Assert.IsFalse(roomScheduleService.IsFree(room,
            new TimeRange(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(2))));
    }

    [TestMethod]
    public void TestIsFreeActuallyFree()
    {
        var room = new Room();
        var examinations = new List<Examination>
        {
            new(null, new Patient(), true, DateTime.Now, room)
        };
        var renovations = new List<Renovation>
        {
            new("", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(5), room)
        };
        var roomScheduleService = new RoomScheduleService(examinations, renovations);

        Assert.IsTrue(roomScheduleService.IsFree(room,
            new TimeRange(DateTime.Now.AddDays(10), DateTime.Now.AddDays(10).AddHours(2))));
    }
}