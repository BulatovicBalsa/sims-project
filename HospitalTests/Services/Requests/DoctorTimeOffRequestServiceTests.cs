using Hospital.Injectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Models.Requests;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Manager;
using Hospital.Repositories.Patient;
using Hospital.Repositories.Requests;
using Hospital.Serialization;
using Hospital.Services.Requests;

namespace HospitalTests.Services.Requests;

[TestClass]
public class DoctorTimeOffRequestServiceTests
{
    [TestInitialize]
    public void SetUp()
    {
        DeleteData();
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

    private void AddData()
    {
        new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).Add(new Doctor("Pera", "Peric", "", "pera", "", "Cardiologist"));
        PatientRepository.Instance.Add(new Patient());
        RoomRepository.Instance.Add(new Room("Examination room", RoomType.ExaminationRoom));
        var room = RoomRepository.Instance.GetAll()[0];
        var patient = PatientRepository.Instance.GetAll()[0];
        var pera = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetByUsername("pera");
        DoctorTimeOffRequestRepository.Instance.Add(new DoctorTimeOffRequest(pera.Id, "reason", DateTime.Now.AddDays(3),
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
        var request = DoctorTimeOffRequestRepository.Instance.GetAll()[0];
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
        var request = DoctorTimeOffRequestRepository.Instance.GetAll()[0];
        service.Approve(request);
        var notificationRepository = new NotificationRepository();
        Assert.AreEqual(1, notificationRepository.GetAll().Count);
    }

    [TestMethod()]
    public void TestReject()
    {
        AddData();
        var service = new DoctorTimeOffRequestService();
        var request = DoctorTimeOffRequestRepository.Instance.GetAll()[0];
        service.Reject(request);
        Assert.AreEqual(0, DoctorTimeOffRequestRepository.Instance.GetAll().Count);
        Assert.AreEqual(3, ExaminationRepository.Instance.GetAll().Count);
    }
}