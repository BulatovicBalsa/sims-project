using Hospital.Injectors;
using Hospital.PatientHealthcare.Repositories;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Patient;
using Hospital.PatientHealthcare.Models;

[TestClass]
public class ExaminationChangesTrackerTests
{
    private ExaminationChangesTracker _examinationChangesTracker;

    private readonly ExaminationChangesTrackerRepository _examinationChangesTrackerRepository =
        new(SerializerInjector.CreateInstance<ISerializer<PatientExaminationLog>>());

    private PatientExaminationLog _log;
    private PatientExaminationLog _oldLog;
    private Patient _patient;
    private PatientExaminationLog _recentLog;

    [TestInitialize]
    public void TestInitialize()
    {
        _examinationChangesTracker = new ExaminationChangesTracker(_examinationChangesTrackerRepository);

        _examinationChangesTrackerRepository.DeleteAll();
        _patient = new Patient("Alice", "Smith", "1234567890124", "alicesmith", "password1",
            new MedicalRecord(80, 180));

        _log = new PatientExaminationLog(_patient, false);

        var now = DateTime.Now;
        _recentLog = new PatientExaminationLog
        {
            Patient = _patient,
            Timestamp = now.AddDays(-10),
            IsCreationLog = false
        };
        _oldLog = new PatientExaminationLog
        {
            Patient = _patient,
            Timestamp = now.AddDays(-35),
            IsCreationLog = false
        };
    }

    [TestMethod]
    public void TestAdd()
    {
        _examinationChangesTracker.Add(_log);

        var logs = _examinationChangesTrackerRepository.GetAll();
        Assert.IsTrue(logs.Contains(_log));
    }

    [TestMethod]
    public void TestGetNumberOfChangeLogsForPatientInLast30Days()
    {
        _examinationChangesTracker.Add(_recentLog);
        _examinationChangesTracker.Add(_oldLog);

        var count = _examinationChangesTracker.GetNumberOfChangeLogsForPatientInLast30Days(_patient);

        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void TestGetNumberOfCreationLogsForPatientInLast30Days()
    {
        _oldLog.IsCreationLog = true;
        _recentLog.IsCreationLog = true;

        _examinationChangesTracker.Add(_recentLog);
        _examinationChangesTracker.Add(_oldLog);

        var count = _examinationChangesTracker.GetNumberOfCreationLogsForPatientInLast30Days(_patient);

        Assert.AreEqual(1, count);
    }
}