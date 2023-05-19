using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
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
                PrescriptionsToModify.Clear();
                _prescriptions.ToList().ForEach(prescription => PrescriptionsToModify.Add(prescription));
                OnPropertyChanged(nameof(Prescriptions));
            }
        }

        public HospitalTreatmentReferral? ReferralToModify { get; set; }

        public List<Prescription> PrescriptionsToModify { get; set; }
        public CreatePrescriptionViewModel(Patient patientOnExamination, HospitalTreatmentReferral? referralToModify=null)
        {
            PatientOnExamination = patientOnExamination;
            ReferralToModify = referralToModify;
            PrescriptionsToModify = referralToModify is null
                ? PatientOnExamination.MedicalRecord.Prescriptions
                : referralToModify.Prescriptions;
            Prescriptions = new ObservableCollection<Prescription>(PrescriptionsToModify);

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
            var dialog = new AddPrescriptionDialog(PatientOnExamination, ReferralToModify);
            dialog.ShowDialog();
            Prescriptions = new ObservableCollection<Prescription>(PrescriptionsToModify);
            _patientService.UpdatePatient(PatientOnExamination);
        }

        public ICommand AddPrescriptionCommand { get; set; }
        public ICommand UpdatePrescriptionCommand { get; set; }
        public ICommand DeletePrescriptionCommand { get; set; }

    }
}
