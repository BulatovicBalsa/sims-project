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
        DoctorRepository.Instance.Add(new Doctor("Pera", "Peric", "", "pera", "", "Cardiologist"));
        PatientRepository.Instance.Add(new Patient());
        RoomRepository.Instance.Add(new Room("Examination room", RoomType.ExaminationRoom));
        var room = RoomRepository.Instance.GetAll()[0];
        var patient = PatientRepository.Instance.GetAll()[0];
        var pera = DoctorRepository.Instance.GetByUsername("pera");
        DoctorTimeOffRequestRepository.Instance.Add(new DoctorTimeOffRequest(pera.Id, "reason", DateTime.Now.AddDays(3), DateTime.Now.AddDays(10)));
        ExaminationRepository.Instance.Add(new Examination(pera, patient, false, DateTime.Now.AddDays(4), room), false);
        ExaminationRepository.Instance.Add(new Examination(pera, patient, false, DateTime.Now.AddDays(11), room), false);
        ExaminationRepository.Instance.Add(new Examination(pera, patient, false, DateTime.Now.AddDays(-1), room), false);
    }

    [TestMethod]
    public void TestAcceptRequestAndCancelExaminations()
    {
        AddData();
        Assert.AreEqual(3, ExaminationRepository.Instance.GetAll().Count);
        var service = new DoctorTimeOffRequestService();
        var request = DoctorTimeOffRequestRepository.Instance.GetAll()[0];
        service.Accept(request);
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
        service.Accept(request);
        var notificationRepository = new NotificationRepository();
        Assert.AreEqual(1, notificationRepository.GetAll().Count);
    }
}