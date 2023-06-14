﻿using Hospital.Injectors;
using Hospital.Models.Doctor;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Manager;
using Hospital.Repositories.Patient;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Examination;

[TestClass]
public class ExaminationRepositoryTests
{
    private DoctorRepository _doctorRepository;
    private Hospital.Models.Examination.Examination _examination;
    private ExaminationRepository _examinationRepository;
    private PatientRepository _patientRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        ExaminationRepository.DeleteAll();
        ExaminationChangesTrackerRepository.DeleteAll();
        DoctorRepository.DeleteAll();
        PatientRepository.DeleteAll();
        RoomRepository.Instance.DeleteAll();
        RoomRepository.Instance.Add(new Room("53454351", "Examination room", RoomType.ExaminationRoom));

        CreateTestExaminationRepository();
        CreateTestExamination();
        _examinationRepository.Add(_examination, true);
    }

    private void CreateTestExaminationRepository()
    {
        _examinationRepository = ExaminationRepository.Instance;
        _doctorRepository = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
        _patientRepository = PatientRepository.Instance;

        _patientRepository.PatientAdded += _ => { };
        _patientRepository.PatientUpdated += _ => { };

        var doctor1 = new Doctor("Dr. Emily", "Brown", "1234567890121", "dremilybrown", "docpassword1", "Cardiologist");
        var doctor2 = new Doctor("Dr. Jake", "Wilson", "1234567890122", "drjakewilson", "docpassword2", "Cardiologist");

        _doctorRepository.Add(doctor1);
        _doctorRepository.Add(doctor2);

        var patient1 = new Hospital.Models.Patient.Patient("Alice", "Smith", "1234567890124", "alicesmith", "password1",
            new MedicalRecord(80, 180));
        var patient2 = new Hospital.Models.Patient.Patient("Bob", "Johnson", "1234567890125", "bobjohnson", "password2",
            new MedicalRecord(80, 180));
        _patientRepository.Add(patient1);
        _patientRepository.Add(patient2);

        var examination1 = new Hospital.Models.Examination.Examination(doctor1, patient1, false,
            DateTime.Now.AddHours(30),
            RoomRepository.Instance.GetAll()[0]);
        var examination2 = new Hospital.Models.Examination.Examination(doctor1, patient2, false,
            DateTime.Now.AddHours(40),
            RoomRepository.Instance.GetAll()[0]);
        var examination3 = new Hospital.Models.Examination.Examination(doctor2, patient1, true,
            DateTime.Now.AddHours(50),
            RoomRepository.Instance.GetAll()[0]);

        _examinationRepository.Add(examination1, true);
        _examinationRepository.Add(examination2, true);
        _examinationRepository.Add(examination3, false);
    }

    private void CreateTestExamination()
    {
        var doctor = new Doctor("Dr. Linda", "Miller", "1234567890123", "drlindamiller", "docpassword3",
            "Cardiologist");
        var patient = new Hospital.Models.Patient.Patient("Charlie", "Williams", "1234567890126", "charliewilliams", "password3",
            new MedicalRecord(80, 180));

        _doctorRepository.Add(doctor);
        _patientRepository.Add(patient);

        _examination = new Hospital.Models.Examination.Examination(doctor, patient, false, DateTime.Now.AddHours(60),
            RoomRepository.Instance.GetAll()[0]);
    }

    [TestMethod]
    public void TestAdd()
    {
        var addedExamination = _examinationRepository.GetById(_examination.Id);
        Assert.IsNotNull(addedExamination);
    }

    [TestMethod]
    public void TestUpdate()
    {
        _examination.Start = _examination.Start.AddMinutes(5);
        _examination.IsOperation = true;

        _examinationRepository.Update(_examination, false);

        var updatedExamination = _examinationRepository.GetById(_examination.Id);

        Assert.IsNotNull(updatedExamination);
        Assert.AreEqual(_examination.Id, updatedExamination.Id);
        Assert.AreEqual(_examination.Doctor, updatedExamination.Doctor);
        Assert.AreEqual(_examination.Patient, updatedExamination.Patient);
        Assert.AreEqual(_examination.IsOperation, updatedExamination.IsOperation);


        const double tolerance = 1; // 1 second
        var secondsDifference = Math.Abs((_examination.Start - updatedExamination.Start).TotalSeconds);
        Assert.IsTrue(secondsDifference <= tolerance, "The Start times are not equal within the tolerance value.");
    }

    [TestMethod]
    public void TestDelete()
    {
        _examinationRepository.Delete(_examination, true);

        var deletedExaminaton = _examinationRepository.GetById(_examination.Id);
        Assert.IsNull(deletedExaminaton);
    }
}