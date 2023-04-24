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
    public class DoctorViewModel: ViewModelBase
    {
        private readonly DoctorCoordinator _coordinator = new DoctorCoordinator();
        private readonly string _placeholder = "Search...";
        private ObservableCollection<Examination> _examinations;

        public ObservableCollection<Examination> Examinations
        {
            get { return _examinations; }
            set { _examinations = value; OnPropertyChanged(nameof(Examinations)); }
        }

        private ObservableCollection<Patient> _patients;

        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { _patients = value; OnPropertyChanged(nameof(Patients)); }
        }

        private Doctor _doctor;

        public Doctor Doctor
        {
            get { return _doctor; }
            set { _doctor = value; OnPropertyChanged(nameof(Doctor)); }
        }

        private string _searchBoxText;

        public string SearchBoxText
        {
            get { return _searchBoxText; }
            set { _searchBoxText = value; OnPropertyChanged(SearchBoxText); }
        }

        private object _selectedPatient;

        public object SelectedPatient
        {
            get { return _selectedPatient; }
            set { _selectedPatient = value; OnPropertyChanged(nameof(SelectedPatient)); }
        }

        private object _selectedExamination;

        public object SelectedExamination
        {
            get { return _selectedExamination; }
            set { _selectedExamination = value; }
        }


        public ICommand BtnViewMedicalRecord_Command { get; set; }
        public ICommand BtnAddExamination_Command { get; set; }
        public ICommand BtnPerformExamination_Command { get; set; }
        public ICommand BtnUpdateExamination_Command { get; set; }
        public ICommand BtnDeleteExamination_Command { get; set; }

        public DoctorViewModel(Doctor doctor) {
            _doctor = doctor;
            Patients = new ObservableCollection<Patient>(_coordinator.GetViewedPatients(doctor));
            Examinations = new ObservableCollection<Examination>(_coordinator.GetExaminationsForNextThreeDays(doctor));
            SearchBoxText = _placeholder;

            BtnViewMedicalRecord_Command = new RelayCommand(ViewMedicalRecord);
            BtnAddExamination_Command = new RelayCommand(AddExamination);
            BtnUpdateExamination_Command = new RelayCommand(UpdateExamination);
            BtnDeleteExamination_Command = new RelayCommand(DeleteExamination);
            BtnPerformExamination_Command = new RelayCommand(PerformExamination);
        }

        private void ViewMedicalRecord()
        {
            Patient? patient = SelectedPatient as Patient;
            if (patient == null)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            var dialog = new MedicalRecordDialog(patient, false);
            dialog.ShowDialog();
        }

        private void AddExamination()
        {
            var dialog = new ModifyExaminationDialog(Doctor, Examinations);
            dialog.WindowStyle = WindowStyle.ToolWindow;

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Succeed");
            }
        }

        private void UpdateExamination()
        {
            Examination? examinationToChange = SelectedExamination as Examination;
            if (examinationToChange == null)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            var dialog = new ModifyExaminationDialog(Doctor, Examinations, examinationToChange);

            dialog.WindowStyle = WindowStyle.ToolWindow;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Succeed");
            }
        }

        private void DeleteExamination()
        {
            Examination? examination = SelectedExamination as Examination;
            if (examination == null)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            try
            {
                _coordinator.DeleteExamination(examination);
            }
            catch (DoctorNotBusyException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (PatientNotBusyException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Examinations.Remove(examination);
            MessageBox.Show("Succeed");
        }

        private void PerformExamination()
        {
            Examination? examinationToPerform = SelectedExamination as Examination;
            if (examinationToPerform == null)
            {
                MessageBox.Show("Please select examination in order to perform it");
                return;
            }

            Patient patientOnExamination = _coordinator.GetPatient(examinationToPerform);

            /*if (!examination.IsPerfomable())
            {
                MessageBox.Show("Chosen examination can't be performed right now");
                return;
            }*/

            var dialog = new PerformExaminationDialog(examinationToPerform, patientOnExamination);
            dialog.ShowDialog();
        }
    }
}
