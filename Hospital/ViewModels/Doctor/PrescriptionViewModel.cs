using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Hospital.Models.Patient;
using Hospital.Services;
using Hospital.Views;

namespace Hospital.ViewModels
{
    public class CreatePrescriptionViewModel : ViewModelBase
    {
        public Patient PatientOnExamination { get; set; }
        private ObservableCollection<Prescription> _prescriptions = new();
        private readonly PatientService _patientService = new();

        private Prescription? _selectedPrescription;

        public Prescription? SelectedPrescription
        {
            get => _selectedPrescription; 
            set { _selectedPrescription = value; OnPropertyChanged(nameof(SelectedPrescription)); }
        }


        public ObservableCollection<Prescription> Prescriptions
        {
            get => _prescriptions;
            set
            {
                _prescriptions = value;
                PatientOnExamination.MedicalRecord.Prescriptions = new List<Prescription>(_prescriptions);
                OnPropertyChanged(nameof(Prescriptions));
            }
        }

        public CreatePrescriptionViewModel(Patient patientOnExamination)
        {
            PatientOnExamination = patientOnExamination;
            Prescriptions = new ObservableCollection<Prescription>(PatientOnExamination.MedicalRecord.Prescriptions);

            AddPrescriptionCommand = new RelayCommand(AddPrescription);
            UpdatePrescriptionCommand = new RelayCommand(UpdatePrescription);
            DeletePrescriptionCommand = new RelayCommand(DeletePrescription);
        }

        private void DeletePrescription()
        {
            if (SelectedPrescription is null)
            {
                MessageBox.Show("You must select prescription in order to delete it","", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Prescriptions.Remove(SelectedPrescription);
            Prescriptions = new ObservableCollection<Prescription>(Prescriptions);
            _patientService.UpdatePatient(PatientOnExamination);
        }

        private void UpdatePrescription()
        {
            throw new NotImplementedException();
        }

        private void AddPrescription()
        {
            var dialog = new AddPrescriptionDialog(PatientOnExamination);
            dialog.ShowDialog();
            Prescriptions = new ObservableCollection<Prescription>(PatientOnExamination.MedicalRecord.Prescriptions);
            _patientService.UpdatePatient(PatientOnExamination);
        }

        public ICommand AddPrescriptionCommand { get; set; }
        public ICommand UpdatePrescriptionCommand { get; set; }
        public ICommand DeletePrescriptionCommand { get; set; }

    }
}
