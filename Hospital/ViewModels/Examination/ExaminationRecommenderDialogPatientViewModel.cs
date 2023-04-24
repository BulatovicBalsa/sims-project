using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    }
}
