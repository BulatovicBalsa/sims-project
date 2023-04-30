using GalaSoft.MvvmLight.Command;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Hospital.ViewModels
{
    public class PatientMedicalRecordViewModel : ViewModelBase
    {
        private Patient _patient;
        private string _searchText;
        private ObservableCollection<Examination> _examinations;
        private PatientMedicalRecordService _patientMedicalRecordService;

        public int Height
        {
            get => _patient.MedicalRecord.Height;
            set
            {
                _patient.MedicalRecord.Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        public int Weight
        {
            get => _patient.MedicalRecord.Weight;
            set
            {
                _patient.MedicalRecord.Weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        public ObservableCollection<string> Allergies => new ObservableCollection<string>(_patient.MedicalRecord.Allergies);
        public ObservableCollection<string> MedicalHistory => new ObservableCollection<string>(_patient.MedicalRecord.MedicalHistory);
        public ObservableCollection<Examination> Examinations
        {
            get => _examinations;
            set
            {
                _examinations = value;
                OnPropertyChanged(nameof(Examinations));
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterExaminations();
            }
        }

        public ICommand SortByDateCommand { get;private set; }
        public ICommand SortByDoctorCommand { get;private set; }
        public ICommand SortBySpecializationCommand { get;private set; }

        public PatientMedicalRecordViewModel(Patient patient)
        {
            _patientMedicalRecordService = new PatientMedicalRecordService();
            _patient = patient;
            _examinations = new ObservableCollection<Examination>(_patientMedicalRecordService.GetPatientExaminations(patient));
            _searchText="Search...";

            SortByDateCommand = new RelayCommand(SortByDate);
            SortByDoctorCommand = new RelayCommand(SortByDoctor);
            SortBySpecializationCommand = new RelayCommand(SortBySpecialization);
        }

        private void SortByDate()
        {
            _examinations = new ObservableCollection<Examination>(Examinations.OrderBy(examination => examination.Start));
            OnPropertyChanged(nameof(Examinations));
        }
        private void SortByDoctor()
        {
            _examinations = new ObservableCollection<Examination>(Examinations.OrderBy(examination => examination.Doctor.LastName));
            OnPropertyChanged(nameof(Examinations));
        }
        private void SortBySpecialization()
        {
            _examinations = new ObservableCollection<Examination>(Examinations.OrderBy(examination => examination.Doctor.Specialization));
            OnPropertyChanged(nameof(Examinations));
        }
        private void FilterExaminations()
        {
            _examinations = new ObservableCollection<Examination>(_patientMedicalRecordService.GetPatientExaminations(_patient));
            _examinations = new ObservableCollection<Examination>(_examinations.Where(examinations => examinations.Anamnesis.Contains(SearchText)));
            OnPropertyChanged(nameof(Examinations));
        }
    }
}
