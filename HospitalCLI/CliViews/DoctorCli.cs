using System.Collections.ObjectModel;
using Hospital.Models.Examination;
using Hospital.Services;
using Hospital.ViewModels;

namespace HospitalCLI.CliViews
{
    public class DoctorCli : DoctorViewModel
    {
        private static readonly DoctorService DoctorService = new();

        public DoctorCli() : base(DoctorService.GetById(Thread.CurrentPrincipal!.Identity!.Name!.Split('|')[0])!)
        {
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
