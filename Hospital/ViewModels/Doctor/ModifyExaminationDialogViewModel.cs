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
        private readonly DoctorCoordinator _coordinator = new DoctorCoordinator();

        private bool _isOperation;

        public bool IsOperation
        {
            get { return _isOperation; }
            set { _isOperation = value; OnPropertyChanged(nameof(IsOperation)); }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }

        private Patient _selectedPatient;

        public Patient SelectedPatient
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


        public ICommand ModifyExamination { get; set; }

        public ModifyExaminationDialogViewModel(Doctor doctor, ObservableCollection<Examination> examinationCollection, Examination examinationToChange = null)
        {
            _isUpdate = !(examinationToChange is null);
            _doctor = doctor;
            _examinationCollection = examinationCollection;
            _examinationToChange = examinationToChange;

            Patients = new ObservableCollection<Patient>(_coordinator.GetAllPatients());
            fillForm();
        }


        private void fillForm()
        {
            SelectedDate = _examinationToChange is null ? DateTime.Now : _examinationToChange.Start;
            IsOperation = _examinationToChange is null ? false : _examinationToChange.IsOperation;
            SelectedPatient = _examinationToChange is null ? null : _examinationToChange.Patient;
            ButtonContent = _examinationToChange is null ? "Create" : "Update";

            ModifyExamination = new RelayCommand<Window>(AddExamination_Click);
        }

        private void AddExamination_Click(Window window)
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
                    _coordinator.AddExamination(createdExamination);
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
            Patient? patient = SelectedPatient as Patient;
            if (patient == null)
            {
                MessageBox.Show("You must select patient");
                return null;
            }

            DateTime? startDate = SelectedDate;
            if (startDate == null)
            {
                MessageBox.Show("You must select date and time");
                return null;
            }

            return new Examination(_doctor, patient, IsOperation, startDate.GetValueOrDefault());
        }

        private void updateExamination(Examination examination)
        {
            examination.Id = _examinationToChange.Id;
            _coordinator.UpdateExamination(examination);
            _examinationCollection.Clear();
            _coordinator.GetExaminationsForNextThreeDays(_doctor).ForEach(examination => _examinationCollection.Add(examination));
        }
    }
}
