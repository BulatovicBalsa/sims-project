using Hospital.Models.Examination;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Repositories.Examinaton;

using Models.Patient;
using Models.Doctor;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using Doctor;
using Patient;
using Exceptions;
using Manager;

public sealed class ExaminationReadMapper : ClassMap<Examination>
{
    public ExaminationReadMapper()
    {
        Map(examination => examination.Id).Index(0);
        Map(examination => examination.IsOperation).Index(1);
        Map(examination => examination.Start).Index(2);

        Map(examination => examination.Doctor).Index(3).TypeConverter<DoctorTypeConverter>();
        Map(examination => examination.Patient).Index(4).TypeConverter<PatientTypeConverter>();
        Map(examination => examination.Anamnesis).Index(5);

        Map(examination => examination.Room).Index(6).TypeConverter<RoomTypeConverter>();
        Map(examination => examination.Admissioned).Index(7);
    }

    private List<string> SplitColumnValues(string? columnValue)
    {
        return columnValue?.Split("|").ToList() ?? new List<string>();
    }

    public class DoctorTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var doctorId = inputText.Trim();
            if (string.IsNullOrEmpty(doctorId))
                return null;
            // Retrieve the Doctor object based on the ID
            var doctor = DoctorRepository.Instance.GetById(doctorId) ??
                         throw new KeyNotFoundException($"Doctor with ID {doctorId} not found");
            return doctor;
        }
    }

    public class PatientTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var patientId = inputText.Trim();
            // Retrieve the Patient object based on the ID
            var patient = new PatientRepository().GetById(patientId) ??
                          throw new KeyNotFoundException($"Patient with ID {patientId} not found");
            return patient;
        }
    }

    public class RoomTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var roomId = inputText.Trim();

            if (string.IsNullOrEmpty(roomId))
                return null;
            // Retrieve the Room object based on the ID
            var room = RoomRepository.Instance.GetById(roomId) ??
                       throw new KeyNotFoundException($"Room with ID {roomId} not found");

            return room;
        }
    }
}

public sealed class ExaminationWriteMapper : ClassMap<Examination>
{
    public ExaminationWriteMapper()
    {
        Map(examination => examination.Id).Index(0);
        Map(examination => examination.IsOperation).Index(1);
        Map(examination => examination.Start).Index(2);
        Map(examination => examination.Doctor!.Id).Index(3);
        Map(examination => examination.Patient!.Id).Index(4);
        Map(examination => examination.Anamnesis).Index(5);
        Map(examination => examination.Room!.Id).Index(6);
        Map(examination => examination.Admissioned).Index(7);
    }
}

public class ExaminationRepository
{
    private const string FilePath = "../../../Data/examination.csv";
    private readonly ExaminationChangesTracker _examinationChangesTracker;

    public ExaminationRepository(ExaminationChangesTracker examinationChangesTracker)
    {
        _examinationChangesTracker = examinationChangesTracker;
    }

    public ExaminationRepository()
    {
        _examinationChangesTracker = new ExaminationChangesTracker();
    }

    public List<Examination> GetAll()
    {
        return Serializer<Examination>.FromCSV(FilePath, new ExaminationReadMapper());
    }

    public Examination? GetById(string id)
    {
        return GetAll().Find(examination => examination.Id == id);
    }

    public void Add(Examination examination, bool isMadeByPatient)
    {
        var allExamination = GetAll();

        if (!IsFree(examination.Doctor, examination.Start)) throw new DoctorBusyException("Doctor is busy");
        if (!IsFree(examination.Patient!, examination.Start)) throw new PatientBusyException("Patient is busy");
        if (isMadeByPatient)
        {
            PatientExaminationLog log = new(examination.Patient!, true);
            _examinationChangesTracker.Add(log);
        }

        allExamination.Add(examination);

        Serializer<Examination>.ToCSV(allExamination, FilePath, new ExaminationWriteMapper());
    }

    public void Update(Examination examination, bool isMadeByPatient)
    {
        var allExamination = GetAll();

        var indexToUpdate = allExamination.FindIndex(e => e.Id == examination.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        if (!IsFree(examination.Doctor, examination.Start, examination.Id))
            throw new DoctorBusyException("Doctor is busy");
        if (!IsFree(examination.Patient!, examination.Start, examination.Id))
            throw new PatientBusyException("Patient is busy");
        if (isMadeByPatient)
        {
            ValidateExaminationTiming(examination.Start);
            ValidateMaxChangesOrDeletesLast30Days(examination.Patient!);
            ValidateMaxAllowedExaminationsLast30Days(examination.Patient!);
            PatientExaminationLog log = new(examination.Patient!, false);
            _examinationChangesTracker.Add(log);
        }

        allExamination[indexToUpdate] = examination;

        Serializer<Examination>.ToCSV(allExamination, FilePath, new ExaminationWriteMapper());
    }

    public void Delete(Examination examination, bool isMadeByPatient)
    {
        var allExamination = GetAll();

        var indexToDelete = allExamination.FindIndex(e => e.Id == examination.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        if (examination.Start != DateTime.MinValue)
        {
            if (IsFree(examination.Doctor, examination.Start))
                throw new DoctorNotBusyException("Doctor is not busy,although he should be");
            if (IsFree(examination.Patient, examination.Start))
                throw new PatientNotBusyException("Patient is not busy,although he should be");
        }

        if (isMadeByPatient)
        {
            ValidateExaminationTiming(examination.Start);
            ValidateMaxChangesOrDeletesLast30Days(examination.Patient!);
            ValidateMaxAllowedExaminationsLast30Days(examination.Patient!);
            PatientExaminationLog log = new(examination.Patient!, false);
            _examinationChangesTracker.Add(log);
        }

        allExamination.RemoveAt(indexToDelete);

        Serializer<Examination>.ToCSV(allExamination, FilePath, new ExaminationWriteMapper());
    }

    public List<Examination> GetAll(Doctor doctor)
    {
        var doctorExaminations = GetAll()
            .Where(examination => examination.Doctor!.Equals(doctor))
            .ToList();
        return doctorExaminations;
    }

    public List<Examination> GetAll(Patient patient)
    {
        var patientExaminations = GetAll()
            .Where(examination => examination.Patient!.Equals(patient))
            .ToList();
        return patientExaminations;
    }

    public List<Examination> GetFinishedExaminations(Doctor doctor)
    {
        var currentTime = DateTime.Now;
        var finishedExaminations = GetAll()
            .Where(examination => examination.Doctor!.Equals(doctor) && examination.Start < currentTime).ToList();
        return finishedExaminations;
    }

    public List<Examination> GetExaminationsForDate(Doctor doctor, DateTime requestedDate)
    {
        return GetAll(doctor).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
    }

    public List<Examination> GetExaminationsForDate(Patient patient, DateTime requestedDate)
    {
        return GetAll(patient).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
    }

    public List<Examination> GetExaminationsForNextThreeDays(Doctor doctor)
    {
        return GetAll(doctor).Where(examination =>
            examination.Start >= DateTime.Now && examination.End <= DateTime.Now.AddDays(2)).ToList();
    }

    public bool IsFree(Doctor? doctor, DateTime start, string? examinationId = null)
    {
        if (doctor == null)
            return true;

        var allExaminations = GetAll(doctor);
        var isAvailable = !allExaminations.Any(examination =>
            examination.Id != examinationId && examination.DoesInterfereWith(start));

        return isAvailable;
    }

    public bool IsFree(Patient patient, DateTime start, string? examinationId = null)
    {
        var allExaminations = GetAll(patient);
        var isAvailable = !allExaminations.Any(examination =>
            examination.Id != examinationId && examination.DoesInterfereWith(start));

        return isAvailable;
    }

    private void ValidateExaminationTiming(DateTime start)
    {
        if (start < DateTime.Now.AddDays(Patient.MINIMUM_DAYS_TO_CHANGE_OR_DELETE_EXAMINATION))
            throw new InvalidOperationException(
                $"It is not possible to update an examination less than {Patient.MINIMUM_DAYS_TO_CHANGE_OR_DELETE_EXAMINATION * 24} hours in advance.");
    }

    private void ValidateMaxChangesOrDeletesLast30Days(Patient patient)
    {
        if (_examinationChangesTracker.GetNumberOfChangeLogsForPatientInLast30Days(patient) + 1 >
            Patient.MAX_CHANGES_OR_DELETES_LAST_30_DAYS)
            throw new InvalidOperationException("Patient made too many changes in last 30 days");
    }

    private void ValidateMaxAllowedExaminationsLast30Days(Patient patient)
    {
        if (_examinationChangesTracker.GetNumberOfCreationLogsForPatientInLast30Days(patient) + 1 >
            Patient.MAX_ALLOWED_APPOINTMENTS_LAST_30_DAYS)
            throw new InvalidOperationException("Patient made too many examinations in last 30 days");
    }

    public static void DeleteAll()
    {
        var emptyList = new List<Examination>();
        Serializer<Examination>.ToCSV(emptyList, FilePath, new ExaminationWriteMapper());
    }
}