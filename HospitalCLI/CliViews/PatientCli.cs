using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCLI.CliViews
{
    public class PatientCli
    {
        private readonly List<Doctor> _allDoctors;
        private readonly Patient _patient;
        private readonly ExaminationRecommenderService _examinationRecommenderService;

        public PatientCli(Patient patient)
        {
            _patient = patient;
            _allDoctors = _examinationRecommenderService.GetAllDoctors();
            _examinationRecommenderService = new ExaminationRecommenderService();
            Menu();
        }

        public void Menu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Operations");
                Console.WriteLine("===============================================");
                Console.WriteLine("1 - View Recommended Examinations");
                Console.WriteLine("2 - Schedule Examination");
                Console.WriteLine("X - Exit");
                Console.WriteLine();
                Console.Write("Choose operation: ");

                var dialogResult = Console.ReadLine();

                switch (dialogResult)
                {
                    case "1":
                        ViewRecommendedExaminations();
                        break;
                    case "2":
                        ScheduleExamination();
                        break;
                    case "X":
                        Console.WriteLine("Good Bye!!!!!!");
                        return;
                }
            }
        }

        private void ViewRecommendedExaminations()
        {
            var recommendedExaminations = _examinationRecommenderService.FindAvailableExaminations(_patient);
            Console.WriteLine("\nRecommended Examinations:");
            recommendedExaminations.ForEach(examination => Console.WriteLine($"\t{examination}"));
        }

        private void ScheduleExamination()
        {
            Console.WriteLine("\nEnter your preferences for scheduling an examination:");
            Console.Write("Doctor's first name: ");
            var doctorFirstName = Console.ReadLine();
            Console.Write("Doctor's last name: ");
            var doctorLastName = Console.ReadLine();
            Console.Write("Preferred date (yyyy-MM-dd): ");
            var preferredDate = Console.ReadLine();
            Console.Write("Preferred time range (HH:mm-HH:mm): ");
            var preferredTimeRange = Console.ReadLine();
            Console.Write("Latest date (yyyy-MM-dd): ");
            var latestDate = Console.ReadLine();
            Console.Write("Priority (Doctor/Time Range): ");
            var priority = Console.ReadLine();

            var options = new ExaminationSearchOptions(preferredDoctor, latestDate, startTime, endTime, (Priority)Enum.Parse(typeof(Priority), priority, true));
            // Perform scheduling logic based on the provided preferences
            // Replace with your implementation

            Console.WriteLine("\nExamination scheduled successfully!");
        }
    }
}
