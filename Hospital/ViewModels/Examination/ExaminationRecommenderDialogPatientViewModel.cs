using GalaSoft.MvvmLight.CommandWpf;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Hospital.ViewModels
{
    internal class ExaminationRecommenderDialogPatientViewModel : ViewModelBase
    {
        private ExaminationRecommenderService _examinationService;
        private Patient _patient;

        public ObservableCollection<Doctor> Doctors { get; set; }  
        public ObservableCollection<Examination> RecommendedExaminations { get; set; }

        public Doctor SelectedDoctor { get; set; }
        public DateTime? LatestDate { get; set; }
        public string StartTimeRange { get; set; }
        public string EndTimeRange { get; set;}
        public int SelectedPriorityIndex { get; set; }

        public ICommand FindCommand { get; set; }
        public ICommand SelectCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public event Action RequestClose;

        public ExaminationRecommenderDialogPatientViewModel(Patient patient)
        {
            _patient = patient;
            _examinationService = new ExaminationRecommenderService();

            Doctors = new ObservableCollection<Doctor>(_examinationService.GetAllDoctors());
            RecommendedExaminations = new ObservableCollection<Examination>();

            FindCommand = new RelayCommand(Find);
            SelectCommand = new RelayCommand(Select);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            RequestClose?.Invoke();
        }

        private void Select()
        {
            throw new NotImplementedException();
        }

        private void Find()
        {
            if (IsInputValid()) return;

            TimeSpan startTime, endTime;

            if (!TimeSpan.TryParse(StartTimeRange, out startTime) || !TimeSpan.TryParse(EndTimeRange, out endTime)) return;

            var options = new ExaminationSearchOptions(SelectedDoctor, LatestDate.Value, startTime, endTime, (Priority)SelectedPriorityIndex);
            var foundExaminations = _examinationService.FindAvailableExaminations(_patient, options);

            RecommendedExaminations.Clear();
            foreach (var examination in foundExaminations) RecommendedExaminations.Add(examination);
        }


        private bool IsInputValid()
        {
            return SelectedDoctor != null && LatestDate != null && StartTimeRange != null && EndTimeRange != null;
        }
    }
}
