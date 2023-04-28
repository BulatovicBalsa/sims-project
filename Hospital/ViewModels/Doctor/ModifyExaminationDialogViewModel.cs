using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels
{
    public class ModifyExaminationDialogViewModel : ViewModelDialogBase
    {
        private Doctor _doctor;
        private bool _isUpdate = false;
        private ObservableCollection<Examination> _examinationCollection;
        private Examination? _examinationToChange = null;
        private readonly DoctorService _doctorService = new DoctorService();

        private bool _isOperation;

        public bool IsOperation
        {
            get { return _isOperation; }
            set { _isOperation = value; OnPropertyChanged(nameof(IsOperation)); }
        }

        private DateTime? _selectedDate;

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }

        private Patient? _selectedPatient;

        public Patient? SelectedPatient
        {
            get { return _selectedPatient; }
            set { _selectedPatient = value; OnPropertyChanged(nameof(SelectedPatient)); }
        }

        private ObservableCollection<Patient> _patients;

        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { _patients = value; OnPropertyChanged(nameof(Patients)); }
        }

        private string _buttonContent;

        public string ButtonContent
        {
            get { return _buttonContent; }
            set { _buttonContent = value; OnPropertyChanged(nameof(ButtonContent)); }
        }

        private TimeOnly? _selectedTime;

        public TimeOnly? SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = value; OnPropertyChanged(nameof(SelectedTime)); }
        }

        private ObservableCollection<TimeOnly> _possibleTimes;

        public ObservableCollection<TimeOnly> PossibleTimes
        {
            get { return _possibleTimes; }
            set { _possibleTimes = value; OnPropertyChanged(nameof(PossibleTimes)); }
        }

        public ICommand ModifyExaminationCommand { get; set; }

        public ModifyExaminationDialogViewModel(Doctor doctor, ObservableCollection<Examination> examinationCollection, Examination examinationToChange = null)
        {
            _isUpdate = !(examinationToChange is null);
            _doctor = doctor;
            _examinationCollection = examinationCollection;
            _examinationToChange = examinationToChange;

            Patients = new ObservableCollection<Patient>(_doctorService.GetAllPatients());
            PossibleTimes = new ObservableCollection<TimeOnly>(getPossibleTimes());
            fillForm();
        }


        private void fillForm()
        {
            SelectedDate = _examinationToChange is null ? DateTime.Now : _examinationToChange.Start;
            IsOperation = _examinationToChange is null ? false : _examinationToChange.IsOperation;
            SelectedPatient = _examinationToChange is null ? null : _examinationToChange.Patient;
            SelectedTime = _examinationToChange is null ? null : TimeOnly.FromTimeSpan(_examinationToChange.Start.TimeOfDay);

            ButtonContent = _examinationToChange is null ? "Create" : "Update";

            ModifyExaminationCommand = new RelayCommand<Window>(ModifyExamination);
        }

        private void ModifyExamination(Window window)
        {
            var createdExamination = createExaminationFromForm();
            if (createdExamination is null) return;

            try
            {
                if (_isUpdate)
                {
                    updateExamination(createdExamination);
                }
                else
                {
                    _doctorService.AddExamination(createdExamination);
                    _examinationCollection.Add(createdExamination);
                }
            }
            catch (Exception ex)
            {
                if (ex is DoctorBusyException || ex is PatientBusyException)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            window.DialogResult = true;
        }

        private Examination? createExaminationFromForm()
        {
            if (SelectedPatient is null)
            {
                MessageBox.Show("You must select patient");
                return null;
            }

            if (SelectedDate is null)
            {
                MessageBox.Show("You must select date");
                return null;
            }

            if (SelectedTime is null)
            {
                MessageBox.Show("You must select time");
                return null;
            }

            TimeOnly startTime = SelectedTime.GetValueOrDefault();
            DateTime startDate = SelectedDate.GetValueOrDefault();
            startDate = startDate.Add(startTime.ToTimeSpan());

            return new Examination(_doctor, SelectedPatient, IsOperation, startDate);
        }

        private void updateExamination(Examination examination)
        {
            examination.Id = _examinationToChange.Id;
            _doctorService.UpdateExamination(examination);
            _examinationCollection.Clear();
            _doctorService.GetExaminationsForNextThreeDays(_doctor).ForEach(examination => _examinationCollection.Add(examination));
        }

        private List<TimeOnly> getPossibleTimes()
        {
            List<TimeOnly> possibleTimes = new List<TimeOnly>();

            for (int hour = 0; hour <= 23; hour++)
            {
                for (int minute = 0; minute < 60; minute += 15)
                {
                    possibleTimes.Add(new TimeOnly(hour, minute));
                }
            }
            return possibleTimes;
        }
    }
}
