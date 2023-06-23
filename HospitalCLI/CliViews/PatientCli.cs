using Hospital.Models.Doctor;
using Hospital.Models.Examination;
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
                Console.WriteLine("1 - Get Recommended Loans");
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

            var endTime = GetEndTime(startTime);
            if (endTime == null) return;

            var priority = GetPriority();
            if (priority == null) return;

            Console.WriteLine("\nSearching for examinations...");

            var options = new ExaminationSearchOptions(selectedDoctor, latestDate.Value, startTime.Value, endTime.Value, priority.Value);

            var availableExaminations = _examinationRecommenderService.FindAvailableExaminations(_patient, options);
            PrintAvailableExaminations(availableExaminations);

            var selectedExamination = GetExaminationSelection(availableExaminations);
            if (selectedExamination == null) return;

            _examinationRecommenderService.AddExamination(selectedExamination);
            Console.WriteLine("\nExamination added successfully!");
        }

        private Doctor GetSelectedDoctor()
        {
            var allDoctors = _examinationRecommenderService.GetAllDoctors();

            Console.WriteLine("Available Doctors:");
            allDoctors.Select((doctor, index) => $"{index + 1}. {doctor.FirstName} {doctor.LastName}").ToList().ForEach(Console.WriteLine);

            while (true)
            {
                Console.Write("Enter the number of the doctor you want to select: ");
                if (!TryParseInput(Console.ReadLine(), out var doctorSelection, 1, allDoctors.Count))
                {
                    Console.WriteLine("Invalid doctor selection. Please enter a valid number.");
                    continue;
                }

                return allDoctors[doctorSelection - 1];
            }
        }

        private DateTime? GetLatestDate()
        {
            while(true)
            {
                Console.Write("Latest date (MM/dd/yyyy): ");
                if (!TryParseDateTime(Console.ReadLine(), "MM/dd/yyyy", out var latestDate))
                {
                    Console.WriteLine("Invalid date format. Please enter the date in the format MM/dd/yyyy.");
                    continue;
                }

                return latestDate;
            }
        }

        private TimeSpan? GetStartTime()
        {
            while (true)
            {
                Console.Write("Start time (HH:mm): ");
                if (!TryParseTimeSpan(Console.ReadLine(), "hh\\:mm", out var startTime))
                {
                    Console.WriteLine("Invalid time format. Please enter the time in the format HH:mm.");
                    continue;
                }

                return startTime;
            }
        }

        private TimeSpan? GetEndTime(TimeSpan? startTime)
        {
            while (true)
            {
                Console.Write("End time (HH:mm): ");
                if (!TryParseTimeSpan(Console.ReadLine(), "hh\\:mm", out var endTime))
                {
                    Console.WriteLine("Invalid time format. Please enter the time in the format HH:mm.");
                    continue;
                }
                if (endTime <= startTime)
                {
                    Console.WriteLine("End time must be after the start time. Please try again.");
                    continue;
                }

                return endTime;
            }
        }

        private Priority? GetPriority()
        {
            while (true)
            {
                Console.Write("Priority (Doctor/Time Range): ");
                if (!TryParseEnum<Priority>(Console.ReadLine(), true, out var priority))
                {
                    Console.WriteLine("Invalid priority. Please enter either 'Doctor' or 'Time Range'.");
                    continue;
                }

                return priority;
            }
        }

        private bool TryParseInput(string input, out int value, int minValue, int maxValue)
        {
            input = input.Trim();
            return int.TryParse(input, out value) && value >= minValue && value <= maxValue;
        }

        private bool TryParseDateTime(string input, string format, out DateTime value)
        {
            input = input.Trim();
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
        }

        private bool TryParseTimeSpan(string input, string format, out TimeSpan value)
        {
            input = input.Trim();
            return TimeSpan.TryParseExact(input, format, CultureInfo.CurrentCulture, out value);
        }

        private bool TryParseEnum<TEnum>(string input, bool ignoreCase, out TEnum value) where TEnum : struct
        {
            input = input.Trim();
            return Enum.TryParse(input, ignoreCase, out value) && Enum.IsDefined(typeof(TEnum), value);
        }
        private Examination GetExaminationSelection(List<Examination> availableExaminations)
        {
            while (true)
            {
                Console.Write("Enter the number of the examination you want to select: ");
                if (!int.TryParse(Console.ReadLine(), out var examinationSelection) || examinationSelection < 1 || examinationSelection > availableExaminations.Count)
                {
                    Console.WriteLine("Invalid examination selection. Please enter a valid number.");
                    continue;
                }
                return availableExaminations[examinationSelection - 1]; ;
            }

        }
        private void PrintAvailableExaminations(IEnumerable<Examination> examinations)
        {
            Console.WriteLine("\nAvailable Loans:");
            examinations.Select((examination, index) => $"{index + 1}. {examination}").ToList().ForEach(Console.WriteLine);
        }
    }
}
