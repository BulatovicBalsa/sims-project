using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCLI.CliViews
{
    public class PatientCli
    {
        private readonly Patient _patient;
        private readonly ExaminationRecommenderService _examinationRecommenderService;

        public PatientCli(Patient patient)
        {
            _patient = patient;
            _examinationRecommenderService = new ExaminationRecommenderService();
            Menu();
        }

        public void Menu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Patient Menu");
                Console.WriteLine("===============================================");
                Console.WriteLine("1 - Get Recommended Examinations");
                Console.WriteLine("X - Exit");
                Console.WriteLine();
                Console.Write("Choose operation: ");

                var dialogResult = Console.ReadLine();

                switch (dialogResult)
                {
                    case "1":
                        GetRecommendedExaminations();
                        break;
                    case "X":
                        Console.WriteLine("Good Bye!!!!!!");
                        return;
                }
            }
        }

        private void GetRecommendedExaminations()
        {
            Console.WriteLine("\nEnter your preferences for finding available examinations:");

            var selectedDoctor = GetSelectedDoctor();
            if (selectedDoctor == null) return;

            var latestDate = GetLatestDate();
            if (latestDate == null) return;

            var startTime = GetStartTime();
            if (startTime == null) return;

            var endTime = GetEndTime();
            if (endTime == null) return;

            var priority = GetPriority();
            if (priority == null) return;

            var options = new ExaminationSearchOptions(selectedDoctor, latestDate.Value, startTime.Value, endTime.Value, priority.Value);

            var availableExaminations = _examinationRecommenderService.FindAvailableExaminations(_patient, options);
            Console.WriteLine("\nAvailable Examinations:");
            availableExaminations.ForEach(examination => Console.WriteLine($"\t{examination}"));
        }

        private Doctor GetSelectedDoctor()
        {
            var allDoctors = _examinationRecommenderService.GetAllDoctors();

            Console.WriteLine("Available Doctors:");
            allDoctors.Select((doctor, index) => $"{index + 1}. {doctor.FirstName} {doctor.LastName}").ToList().ForEach(Console.WriteLine);

            Console.Write("Enter the number of the doctor you want to select: ");
            if (!TryParseInput(Console.ReadLine(), out var doctorSelection, 1, allDoctors.Count))
            {
                Console.WriteLine("Invalid doctor selection. Please enter a valid number.");
                return null;
            }

            return allDoctors[doctorSelection - 1];
        }

        private DateTime? GetLatestDate()
        {
            Console.Write("Latest date (MM/dd/yyyy): ");
            if (!TryParseDateTime(Console.ReadLine(), "MM/dd/yyyy", out var latestDate))
            {
                Console.WriteLine("Invalid date format. Please enter the date in the format MM/dd/yyyy.");
                return null;
            }

            return latestDate;
        }

        private TimeSpan? GetStartTime()
        {
            Console.Write("Start time (HH:mm): ");
            if (!TryParseTimeSpan(Console.ReadLine(), "HH:mm", out var startTime))
            {
                Console.WriteLine("Invalid time format. Please enter the time in the format HH:mm.");
                return null;
            }

            return startTime;
        }

        private TimeSpan? GetEndTime()
        {
            Console.Write("End time (HH:mm): ");
            if (!TryParseTimeSpan(Console.ReadLine(), "HH:mm", out var endTime))
            {
                Console.WriteLine("Invalid time format. Please enter the time in the format HH:mm.");
                return null;
            }

            return endTime;
        }

        private Priority? GetPriority()
        {
            Console.Write("Priority (Doctor/Time Range): ");
            if (!TryParseEnum<Priority>(Console.ReadLine(), true, out var priority))
            {
                Console.WriteLine("Invalid priority. Please enter either 'Doctor' or 'Time Range'.");
                return null;
            }

            return priority;
        }

        private bool TryParseInput(string input, out int value, int minValue, int maxValue)
        {
            return int.TryParse(input, out value) && value >= minValue && value <= maxValue;
        }

        private bool TryParseDateTime(string input, string format, out DateTime value)
        {
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
        }

        private bool TryParseTimeSpan(string input, string format, out TimeSpan value)
        {
            return TimeSpan.TryParseExact(input, format, CultureInfo.InvariantCulture, out value);
        }

        private bool TryParseEnum<TEnum>(string input, bool ignoreCase, out TEnum value) where TEnum : struct
        {
            return Enum.TryParse(input, ignoreCase, out value) && Enum.IsDefined(typeof(TEnum), value);
        }
    }
}
