﻿using System.Collections.ObjectModel;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Services;
using Hospital.Services.Manager;
using Hospital.ViewModels;

namespace HospitalCLI.CliViews
{
    public class DoctorCli : DoctorViewModel
    {
        private static readonly DoctorService DoctorService = new();
        private readonly List<Patient> _allPatients;
        private readonly List<Room> _allRooms;

        public DoctorCli() : base(DoctorService.GetById(Thread.CurrentPrincipal!.Identity!.Name!.Split('|')[0])!)
        {
            _allPatients = _patientService.GetAllPatients();
            _allRooms = new RoomFilterService().GetRoomsForExamination();
            Menu();
        }

        public void Menu()
        {
            while (true)
            {
                Console.WriteLine("1 - View Examinations for next 3 days");
                Console.WriteLine("2 - View Examinations for selected date");
                Console.WriteLine("3 - Add Examination");
                Console.WriteLine("4 - Update Examination");
                Console.WriteLine("5 - Delete Examination");
                Console.WriteLine("X - Exit");

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
                        break;
                    case "5":
                        break;
                    case "X":
                        Console.WriteLine("Good Bye!!!!!!");
                        return;
                }
            }
        }

        private void AddExaminationCli()
        {
            var i = 0;
            var j = 0;

            Console.Write("Date and time: (yyyy-MM-dd HH:mm)");
            var dateAsString = Console.ReadLine();
            if (!DateTime.TryParseExact(dateAsString, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out var selectedDate)) return;
            
            Console.WriteLine("Patients: ");
            _allPatients.ForEach(patient => Console.WriteLine($"{i++}, {patient}"));

            Console.Write("Enter patient index: ");
            var index = Convert.ToInt32(Console.ReadLine());
            var selectedPatient = _allPatients[index];

            Console.WriteLine("Rooms: ");
            _allRooms.ForEach(room => Console.WriteLine($"{j++} {room}"));

            Console.Write("Enter room index: ");
            index = Convert.ToInt32(Console.ReadLine());
            var selectedRoom = _allRooms[index];

            var isOperation = false;
            Console.WriteLine("Is operation?: ");
            if (Console.ReadLine() == "yes") isOperation = true;

            var examination =
                new Examination(_doctor, selectedPatient, isOperation, selectedDate, selectedRoom);

            try
            {
                _examinationService.AddExamination(examination, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ViewExaminationsForNextThreeDays()
        {
            Examinations =
                new ObservableCollection<Examination>(_examinationService.GetExaminationsForNextThreeDays(_doctor));
            Examinations.ToList().ForEach(Console.WriteLine);
        }

        private void ViewExaminationsForSelectedDate()
        {
            Console.WriteLine("Enter date you want to see (yyyy MM dd): ");
            var dateAsString = Console.ReadLine();
            if (!DateTime.TryParseExact(dateAsString, "yyyy MM dd", null, System.Globalization.DateTimeStyles.None, out var selectedDate)) return;
            SelectedDate = selectedDate;
            Examinations.ToList().ForEach(Console.WriteLine);
        }
    }
}
