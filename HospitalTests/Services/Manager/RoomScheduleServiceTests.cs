﻿using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Scheduling;
using Hospital.Services.Manager;

namespace HospitalTests.Models.Manager;

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