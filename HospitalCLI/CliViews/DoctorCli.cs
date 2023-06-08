using System.Globalization;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;
using Hospital.Repositories.Nurse;
using Hospital.Services;
using Hospital.Services.Manager;

namespace HospitalCLI.CliViews;

public class DoctorCli
{
    private readonly List<Patient> _allPatients;
    private readonly List<Room> _allRooms;
    private readonly List<Doctor> _allDoctors;
    private readonly List<Nurse> _allNurses;

    private readonly Doctor _doctor;
    private readonly DoctorService _doctorService = new();
    private readonly ExaminationService _examinationService = new();
    private readonly PatientService _patientService = new();
    private readonly RoomFilterService _roomFilterService = new();

    private List<Examination> _examinations;

    public DoctorCli()
    {
        _doctor = _doctorService.GetById(Thread.CurrentPrincipal!.Identity!.Name!.Split('|')[0])!;

        _allPatients = _patientService.GetAllPatients();
        _allRooms = _roomFilterService.GetRoomsForExamination();
        _allDoctors = _doctorService.GetAll();
        _allNurses = NurseRepository.Instance.GetAll(); //change later
        _examinations = _examinationService.GetExaminationsForNextThreeDays(_doctor);

        _allDoctors.Remove(_allDoctors.FirstOrDefault(_doctor.Equals)!);
        Menu();
    }

    public void Menu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Operations");
            Console.WriteLine("===============================================");
            Console.WriteLine("1 - View Examinations for next 3 days");
            Console.WriteLine("2 - View Examinations for selected date");
            Console.WriteLine("3 - Add Examination");
            Console.WriteLine("4 - Update Examination");
            Console.WriteLine("5 - Delete Examination");
            Console.WriteLine("X - Exit");
            Console.WriteLine();
            Console.Write("Choose operation: ");

            var dialogResult = Console.ReadLine();

            switch (dialogResult)
            {
                case "1":
                    ViewExaminationsForNextThreeDays();
                    break;
                case "2":
                    ViewExaminationsForSelectedDate();
                    break;
                case "3":
                    AddExaminationCli();
                    break;
                case "4":
                    UpdateExaminationCli();
                    break;
                case "5":
                    DeleteExaminationCli();
                    break;
                case "X":
                    Console.WriteLine("Good Bye!!!!!!");
                    return;
            }
        }
    }

    private void UpdateExaminationCli()
    {
        var upcoming = _examinationService.GetUpcomingExaminations(_doctor);
        var i = 0;

        if (upcoming.Count == 0)
        {
            Console.WriteLine("\nYou have no examinations to delete");
            return;
        }

        upcoming.ForEach(examination => Console.WriteLine($"\t{i++} {examination.ToStringCli}"));
        Console.Write("\nExamination index to update: ");
        i = Convert.ToInt32(Console.ReadLine());

        Console.Write("\nChoose new date and time (yyyy-MM-dd HH:mm): ");
        var dateAsString = Console.ReadLine();
        if (!DateTime.TryParseExact(dateAsString, "yyyy-MM-dd HH:mm", null, DateTimeStyles.None,
                out var selectedDate)) return;

        upcoming[i].Start = selectedDate;

        try
        {
            _examinationService.UpdateExamination(upcoming[i], false);
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.Message);
            return;
        }

        Console.WriteLine("\nSucceed");
    }

    private void DeleteExaminationCli()
    {
        var upcoming = _examinationService.GetUpcomingExaminations(_doctor);
        var i = 0;

        if (upcoming.Count == 0)
        {
            Console.WriteLine("\nYou have no examinations to delete");
            return;
        }

        upcoming.ForEach(examination => Console.WriteLine($"\t{i++} {examination.ToStringCli}"));
        Console.Write("\nExamination index to delete: ");
        i = Convert.ToInt32(Console.ReadLine());

        try
        {
            _examinationService.DeleteExamination(upcoming[i], false);
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n" + ex.Message);
            return;
        }

        Console.WriteLine("\nSucceed");
    }

    private void AddExaminationCli()
    {
        var i = 0;
        var j = 0;

        Console.Write("\nDate and time: (yyyy-MM-dd HH:mm): ");
        var dateAsString = Console.ReadLine();
        if (!DateTime.TryParseExact(dateAsString, "yyyy-MM-dd HH:mm", null, DateTimeStyles.None,
                out var selectedDate)) return;

        Console.WriteLine("\nPatients: ");
        _allPatients.ForEach(patient => Console.WriteLine($"\t{i++}, {patient}"));

        Console.Write("\nEnter patient index: ");
        var index = Convert.ToInt32(Console.ReadLine());
        var selectedPatient = _allPatients[index];

        Console.WriteLine("\nRooms: ");
        _allRooms.ForEach(room => Console.WriteLine($"\t{j++} {room}"));

        Console.Write("\nEnter room index: ");
        index = Convert.ToInt32(Console.ReadLine());
        var selectedRoom = _allRooms[index];

        var isOperation = false;
        Console.Write("\nIs operation?: ");
        if (Console.ReadLine() == "yes") isOperation = true;

        List<Doctor> procedureDoctors = null;
        List<Nurse> procedureNurses = null;

        if (isOperation)
        {
            procedureNurses = new();
            procedureDoctors = new();
        }

        i = j = 0;

        while (isOperation)
        {

            Console.WriteLine("\nAvailable doctors: ");
            _allDoctors.ForEach(doctor => Console.WriteLine($"\t{i++} {doctor}"));

            Console.Write("Enter doctor index (X to stop): " );
            var answer = Console.ReadLine();
            if (answer == "X") break;

            procedureDoctors!.Add(_allDoctors[Convert.ToInt32(answer)]);
        }

        while (isOperation)
        {
            Console.WriteLine("\nAvailable nurses: ");
            _allNurses.ForEach(nurse => Console.WriteLine($"\t{j++} {nurse}"));

            Console.Write("Enter nurse index for help (X to stop): ");
            var answer = Console.ReadLine();
            if (answer == "X") break;

            procedureNurses!.Add(_allNurses[Convert.ToInt32(answer)]);
        }

        var examination =
            new Examination(_doctor, selectedPatient, isOperation, selectedDate, selectedRoom, false, procedureDoctors, procedureNurses);

        try
        {
            _examinationService.AddExamination(examination, false);
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.Message);
            return;
        }

        Console.WriteLine("\nSucceed");
    }

    private void ViewExaminationsForNextThreeDays()
    {
        _examinations = _examinationService.GetExaminationsForNextThreeDays(_doctor);
        Console.WriteLine("\nExaminations: ");
        _examinations.ForEach(examination => Console.WriteLine($"\t{examination.ToStringCli}"));
    }

    private void ViewExaminationsForSelectedDate()
    {
        Console.Write("\nEnter date you want to see (yyyy MM dd): ");
        var dateAsString = Console.ReadLine();
        if (!DateTime.TryParseExact(dateAsString, "yyyy MM dd", null, DateTimeStyles.None,
                out var selectedDate)) return;

        _examinations = _examinationService.GetExaminationsForDate(_doctor, selectedDate);
        Console.WriteLine("\nExaminations: ");
        _examinations.ForEach(examination => Console.WriteLine($"\t{examination.ToStringCli}"));
    }
}