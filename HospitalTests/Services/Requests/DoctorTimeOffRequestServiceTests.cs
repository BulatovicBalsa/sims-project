﻿using Hospital.Injectors;
using Hospital.Notifications.Repositories;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
using Hospital.Serialization;
using Hospital.TimeOffRequests.Models;
using Hospital.TimeOffRequests.Repositories;
using Hospital.TimeOffRequests.Services;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace HospitalTests.Services.Requests;

[TestClass]
public class DoctorTimeOffRequestServiceTests
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

    private void AddData()
    {
        new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).Add(new Doctor("Pera", "Peric",
            "", "pera", "", "Cardiologist"));
        PatientRepository.Instance.Add(new Patient());
        RoomRepository.Instance.Add(new Room("Examination room", RoomType.ExaminationRoom));
        var room = RoomRepository.Instance.GetAll()[0];
        var patient = PatientRepository.Instance.GetAll()[0];
        var pera = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetByUsername("pera");
        new DoctorTimeOffRequestRepository(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>()).Add(
            new DoctorTimeOffRequest(pera.Id, "reason", DateTime.Now.AddDays(3),
                DateTime.Now.AddDays(10)));
        ExaminationRepository.Instance.Add(new Examination(pera, patient, false, DateTime.Now.AddDays(4), room), false);
        ExaminationRepository.Instance.Add(new Examination(pera, patient, false, DateTime.Now.AddDays(11), room),
            false);
        ExaminationRepository.Instance.Add(new Examination(pera, patient, false, DateTime.Now.AddDays(-1), room),
            false);
    }

    [TestMethod]
    public void TestAcceptRequestAndCancelExaminations()
    {
        AddData();
        Assert.AreEqual(3, ExaminationRepository.Instance.GetAll().Count);
        var service = new DoctorTimeOffRequestService();
        var request =
            new DoctorTimeOffRequestRepository(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>())
                .GetAll()[0];
        service.Approve(request);
        Assert.AreEqual(2, ExaminationRepository.Instance.GetAll().Count);
        Assert.IsTrue(ExaminationRepository.Instance.GetAll()[0].Start > request.End);
    }

    [TestMethod]
    public void TestAcceptRequestAndNotifyPatients()
    {
        AddData();
        Assert.AreEqual(3, ExaminationRepository.Instance.GetAll().Count);
        var service = new DoctorTimeOffRequestService();
        var request =
            new DoctorTimeOffRequestRepository(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>())
                .GetAll()[0];
        service.Approve(request);
        var notificationRepository = new NotificationRepository();
        Assert.AreEqual(1, notificationRepository.GetAll().Count);
    }

    [TestMethod]
    public void TestReject()
    {
        AddData();
        var service = new DoctorTimeOffRequestService();
        var request =
            new DoctorTimeOffRequestRepository(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>())
                .GetAll()[0];
        service.Reject(request);
        Assert.AreEqual(0,
            new DoctorTimeOffRequestRepository(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>())
                .GetAll().Count);
        Assert.AreEqual(3, ExaminationRepository.Instance.GetAll().Count);
    }
}