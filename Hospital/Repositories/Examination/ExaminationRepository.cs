using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Exceptions;
using Hospital.Filter;
using Hospital.Injectors;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Manager;
using Hospital.Repositories.Librarian;
using Hospital.Repositories.Patient;
using Hospital.Scheduling;
using Hospital.Serialization;
using Hospital.Serialization.Mappers;

namespace Hospital.Repositories.Examination;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Models.Doctor;
using Hospital.Models.Librarian;

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
        Map(examination => examination.Urgent).Index(8);
        Map(examination => examination.ProcedureDoctors).Index(9).TypeConverter<DoctorListTypeConverter>();
        Map(examination => examination.ProcedureLibrarians).Index(10).TypeConverter<LibrarianListTypeConverter>();
    }

    public class DoctorTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var doctorId = inputText?.Trim();
            if (string.IsNullOrEmpty(doctorId))
                return null;
            // Retrieve the Doctor object based on the ID
            var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(doctorId) ??
                         throw new KeyNotFoundException($"Doctor with ID {doctorId} not found");
            return doctor;
        }
    }

    public class PatientTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var patientId = inputText?.Trim();
            // Retrieve the Patient object based on the ID
            var patient = PatientRepository.Instance.GetById(patientId) ??
                          throw new KeyNotFoundException($"Patient with ID {patientId} not found");
            return patient;
        }
    }

    public class RoomTypeConverter : DefaultTypeConverter
   {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var roomId = inputText?.Trim();

            if (string.IsNullOrEmpty(roomId))
                return null;
            // Retrieve the Room object based on the ID
            var room = RoomRepository.Instance.GetById(roomId) ??
                       throw new KeyNotFoundException($"Room with ID {roomId} not found");

            return room;
        }
    }

    public class DoctorListTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            if (string.IsNullOrEmpty(inputText))
                return null;

            var doctors = new List<Doctor>();
            inputText?.Split("|").ToList().ForEach(doctorId =>
            {
                var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(doctorId) ??
                             throw new KeyNotFoundException($"Doctor with ID {doctorId} not found");
                doctors.Add(doctor);
            });
            
            return doctors;
        }
    }

    public class LibrarianListTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            if (string.IsNullOrEmpty(inputText))
                return null;

            var librarians = new List<Librarian>();
            inputText?.Split("|").ToList().ForEach(librarianId =>
            {
                var doctor = LibrarianRepository.Instance.GetById(librarianId) ??
                             throw new KeyNotFoundException($"Doctor with ID {librarianId} not found");
                librarians.Add(doctor);
            });

            return librarians;
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
        Map(examination => examination.Urgent).Index(8);
        Map(examination => examination.ProcedureDoctors).Index(9).Convert(row => string.Join("|", row.Value.ProcedureDoctors?.Select(doctor => doctor.Id) ?? new List<string>())).Index(9);
        Map(examination => examination.ProcedureLibrarians).Index(10).Convert(row => string.Join("|", row.Value.ProcedureLibrarians?.Select(librarian  => librarian.Id) ?? new List<string>())).Index(10);
    }
}

public class ExaminationRepository
{
    private const string FilePath = "../../../Data/examination.csv";
    private readonly ExaminationChangesTracker _examinationChangesTracker;
    private static ExaminationRepository? _instance;

    public static ExaminationRepository Instance => _instance ??= new ExaminationRepository();
    private ExaminationRepository()
    {
        _examinationChangesTracker = new ExaminationChangesTracker();
    }

    public List<Examination> GetAll()
    {
        return CsvSerializer<Examination>.FromCSV(FilePath, new ExaminationReadMapper());
    }

    public Examination? GetById(string id)
    {
        return GetAll().Find(examination => examination.Id == id);
    }

    public void Add(Examination examination, bool isMadeByPatient)
    {
        var allExamination = GetAll();

        if (!IsFree(examination.Doctor, examination.Start)) throw new DoctorBusyException("Doctor is busy");
        if (!IsFree(examination.Patient!, examination.Start)) throw new BookAlreadyLoanedException("Patient is busy");
        if (isMadeByPatient)
        {
            PatientExaminationLog log = new(examination.Patient!, true);
            _examinationChangesTracker.Add(log);
        }

        allExamination.Add(examination);

        CsvSerializer<Examination>.ToCSV(allExamination, FilePath, new ExaminationWriteMapper());
    }

    public void Update(Examination examination, bool isMadeByPatient)
    {
        var allExamination = GetAll();

        var indexToUpdate = allExamination.FindIndex(e => e.Id == examination.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        if (!IsFree(examination.Doctor, examination.Start, examination.Id))
            throw new DoctorBusyException("Doctor is busy");
        if (!IsFree(examination.Patient!, examination.Start, examination.Id))
            throw new BookAlreadyLoanedException("Patient is busy");
        if (isMadeByPatient)
        {
            ValidateExaminationTiming(examination.Start);
            ValidateMaxChangesOrDeletesLast30Days(examination.Patient!);
            ValidateMaxAllowedExaminationsLast30Days(examination.Patient!);
            PatientExaminationLog log = new(examination.Patient!, false);
            _examinationChangesTracker.Add(log);
        }

        allExamination[indexToUpdate] = examination;

        CsvSerializer<Examination>.ToCSV(allExamination, FilePath, new ExaminationWriteMapper());
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

        CsvSerializer<Examination>.ToCSV(allExamination, FilePath, new ExaminationWriteMapper());
    }

    public void Delete(Doctor doctor, TimeRange timeRange)
    {
        foreach (var examination in GetExaminationsInTimeRange(doctor, timeRange))
        {
            Delete(examination, false);
        }
    }

    public List<Examination> GetAll(Doctor doctor)
    {
        var doctorExaminations = GetAll()
            .Where(examination => examination.Doctor?.Equals(doctor) ?? false)
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
            .Where(examination => (examination.Doctor?.Equals(doctor) ?? false)&& examination.Start < currentTime).ToList();
        return finishedExaminations;
    }

    public List<Examination> GetExaminationsForDate(Doctor doctor, DateTime requestedDate)
    {
        return GetAll(doctor).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
    }


    public List<Examination> GetExaminationsInTimeRange(Doctor doctor, TimeRange range)
    {
        return GetAll(doctor).Where(examination => range.DoesOverlapWith(new TimeRange(examination.Start, examination.End))).ToList();
    }

    public List<Examination> GetExaminationsForDate(Patient patient, DateTime requestedDate)
    {
        return GetAll(patient).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
    }

    public List<Examination> GetExaminationsForNextThreeDays(Doctor doctor)
    {
        var filter = new DoctorExaminationsFilter();

        var start = DateTime.Now;
        var end = start.AddDays(2);
        var examinations = GetAll(doctor);

        return filter.Filter(examinations, new ExaminationPerformingSpecification(start, end));
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
        if (start < DateTime.Now.AddDays(Patient.MinimumDaysToChangeOrDeleteExamination))
            throw new InvalidOperationException(
                $"It is not possible to update an examination less than {Patient.MinimumDaysToChangeOrDeleteExamination * 24} hours in advance.");
    }

    private void ValidateMaxChangesOrDeletesLast30Days(Patient patient)
    {
        if (_examinationChangesTracker.GetNumberOfChangeLogsForPatientInLast30Days(patient) + 1 >
            Patient.MaxChangesOrDeletesLast30Days)
            throw new InvalidOperationException("Patient made too many changes in last 30 days");
    }

    private void ValidateMaxAllowedExaminationsLast30Days(Patient patient)
    {
        if (_examinationChangesTracker.GetNumberOfCreationLogsForPatientInLast30Days(patient) + 1 >
            Patient.MaxAllowedExaminationsLast30Days)
            throw new InvalidOperationException("Patient made too many examinations in last 30 days");
    }

    public static void DeleteAll()
    {
        var emptyList = new List<Examination>();
        CsvSerializer<Examination>.ToCSV(emptyList, FilePath, new ExaminationWriteMapper());
    }
}