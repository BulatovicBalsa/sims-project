using Hospital.Core.Notifications.Services;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.Core.Scheduling.Services;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Services;
using Hospital.Notifications.Models;

namespace HospitalCLI.CliViews;

public class NurseCli
{
    private readonly DoctorService _doctorService;
    private readonly PatientRepository _patientRepository;
    private readonly TimeslotService _timeslotService;
    private readonly ExaminationRepository _examinationRepository;
    private readonly ExaminationService _examinationService;
    private readonly NotificationService _notificationService;
    private Patient _selectedPatient;
    private string _selectedSpecialization;
    private bool _isOperation;
    private SortedDictionary<DateTime, Doctor> _earliestFreeTimeslotDoctors;

    private List<Patient> _allPatients;
    private List<String> _allSpecializations;

    public NurseCli()
    {
        _doctorService = new DoctorService();
        _patientRepository = PatientRepository.Instance;
        _timeslotService = new TimeslotService();
        _examinationRepository = ExaminationRepository.Instance;
        _examinationService = new ExaminationService();
        _notificationService = new NotificationService();

        _allPatients = _patientRepository.GetAll();
        _allSpecializations = _doctorService.GetAllSpecializations();
    }

    public void Menu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Urgent examinations");
            Console.WriteLine("===============================================");
            Console.WriteLine("1 - Schedule an urgent examination");
            Console.WriteLine("X - Exit");
            Console.WriteLine();
            Console.Write("Choose operation: ");

            var dialogResult = Console.ReadLine();

            switch (dialogResult)
            {
                case "1":
                    ScheduleUrgentExamination();
                    break;
                case "X":
                case "x":
                    Console.WriteLine("Good Bye!!!!!!");
                    return;
                default:
                    Console.WriteLine("Non-existent operation!");
                    break;
            }
        }
    }

    private void PrintAllPatients()
    {
        for (var i = 0; i < _allPatients.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_allPatients[i]}");
        }
    }

    private void PrintAllSpecializations()
    {
        for (var i = 0; i < _allSpecializations.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_allSpecializations[i]}");
        }
    }

    private void PrintAllExaminations(List<Examination> examinations)
    {
        for (int i = 0; i < examinations.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {examinations[i]}");
        }
    }

    private void ScheduleUrgentExamination()
    {
        PrintAllPatients();
        Console.WriteLine("\nChoose a patient: ");
        string selectedPatientInput;
        int selectedPatientNumber;
        var badInput = false;
        do
        {
            if (badInput)
            {
                Console.WriteLine("Bad input!");
            }
            selectedPatientInput = Console.ReadLine();
            badInput = true;
        } while (!int.TryParse(selectedPatientInput, out selectedPatientNumber) || (selectedPatientNumber <= 0 || selectedPatientNumber > _allPatients.Count));

        PrintAllSpecializations();
        Console.WriteLine("\nChoose a specialization: ");
        string selectedSpecializationInput;
        int selectedSpecializationNumber;
        badInput = false;
        do
        {
            if (badInput)
            {
                Console.WriteLine("Bad input!");
            }
            selectedSpecializationInput = Console.ReadLine();
            badInput = true;
        } while (!int.TryParse(selectedSpecializationInput, out selectedSpecializationNumber) || (selectedSpecializationNumber <= 0 || selectedSpecializationNumber > _allSpecializations.Count));

        Console.WriteLine("\nIs operation (y/N): ");
        _isOperation = Console.ReadLine() == "y";

        _selectedPatient = _allPatients[selectedPatientNumber - 1];
        _selectedSpecialization = _allSpecializations[selectedPatientNumber - 1];

        var qualifiedDoctors = _doctorService.GetQualifiedDoctors(_selectedSpecialization);
        _earliestFreeTimeslotDoctors = _timeslotService.GetEarliestFreeTimeslotDoctors(qualifiedDoctors);

        if (CreateUrgentExamination(_earliestFreeTimeslotDoctors))
            return;

        PostponeExamination(_earliestFreeTimeslotDoctors);

        Console.WriteLine();
    }

    private bool CreateUrgentExamination(SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var earliestFreeTimeslotDoctor = earliestFreeTimeslotDoctors.First();
        if (!TimeslotService.IsIn2Hours(earliestFreeTimeslotDoctor.Key)) return false;

        SaveUrgentExamination(false, earliestFreeTimeslotDoctor.Key, earliestFreeTimeslotDoctor.Value);
        return true;
    }

    private void PostponeExamination(SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var postponableExaminations = _examinationService.GetFivePostponableExaminations(earliestFreeTimeslotDoctors);
        if (postponableExaminations.Count == 0)
        {
            Console.WriteLine("There are no examinations that can be postponed");
            return;
        }

        PrintAllExaminations(postponableExaminations);
        Console.WriteLine("\nChoose an examination to postpone: ");
        string selectedExaminationInput;
        int selectedExaminationNumber;
        var badInput = false;
        do
        {
            if (badInput)
            {
                Console.WriteLine("Bad input!");
            }
            selectedExaminationInput = Console.ReadLine();
            badInput = true;
        } while (!int.TryParse(selectedExaminationInput, out selectedExaminationNumber) || (selectedExaminationNumber <= 0 || selectedExaminationNumber > postponableExaminations.Count));
        
        var selectedExamination = postponableExaminations[selectedExaminationNumber - 1];

        ExecutePostponeExaminationCommand(selectedExamination);
    }

    private void SaveUrgentExamination(bool cancelled, DateTime? newTimeslot, Doctor? freeDoctor)
    {
        if (cancelled)
            return;

        if (_examinationService.IsPatientBusy(_selectedPatient, newTimeslot ?? DateTime.MinValue))
        {
            Console.WriteLine("Patient already has an examination at given time");
            return;
        }

        _examinationRepository.Add(new Examination(freeDoctor, _selectedPatient, _isOperation, newTimeslot ?? DateTime.MinValue, null, true), false);
        SendDoctorNotification(freeDoctor, newTimeslot ?? DateTime.MinValue);
        Console.WriteLine("Urgent examination successfully created!");
    }

    private void SendDoctorNotification(Doctor doctor, DateTime timeslot)
    {
        var notification = new Notification(doctor.Id, $"New urgent examination scheduled at {timeslot}");
        _notificationService.Send(notification);
    }

    private void ExecutePostponeExaminationCommand(Examination selectedExamination)
    {
        var previousStart = selectedExamination.Start;

        if (_examinationService.IsPatientBusy(_selectedPatient, previousStart))
        {
            SaveUrgentExamination(false, previousStart, selectedExamination.Doctor);
            return;
        }

        PostponeExaminationTimestamp(selectedExamination);
        SendNotifications(selectedExamination);

        SaveUrgentExamination(false, previousStart, selectedExamination.Doctor);
    }

    private void PostponeExaminationTimestamp(Examination selectedExamination)
    {
        var doctorEarliestFreeTimeslot = _earliestFreeTimeslotDoctors.ToDictionary(pair => pair.Value, pair => pair.Key);
        selectedExamination.Start = doctorEarliestFreeTimeslot[selectedExamination.Doctor];
        _examinationRepository.Update(selectedExamination, false);
    }

    private void SendNotifications(Examination examination)
    {
        var patientNotification = new Notification(examination.Patient.Id,
            $"Examination {examination.Id} postponed to {examination.Start}");
        var doctorNotification = new Notification(examination.Doctor.Id,
            $"Examination {examination.Id} postponed to {examination.Start}");

        _notificationService.Send(patientNotification);
        _notificationService.Send(doctorNotification);
    }
}
