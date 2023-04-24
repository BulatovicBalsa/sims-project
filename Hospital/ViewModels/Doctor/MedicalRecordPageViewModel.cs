using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Patient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Hospital.ViewModels
{
    public class MedicalRecordPageViewModel : ViewModelBase
    {

        private readonly DoctorCoordinator _doctorCoordinator = new DoctorCoordinator();

        private Patient _patient;

        public Patient Patient
        {
            get { return _patient; }
            set { _patient = value; OnPropertyChanged(nameof(Patient));}
        }

        private string _height;

        public string Height //Maybe rename it
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(nameof(Height)); }
        }

        private string _weight;

        public string Weight
        {
            get { return _weight; }
            set { _weight = value; OnPropertyChanged(nameof(Weight)); }
        }

        private string _selectedAllergy;

        public string SelectedAllergy
        {
            get { return _selectedAllergy; }
            set { _selectedAllergy = value; OnPropertyChanged(nameof(SelectedAllergy)); }
        }

        private string _selectedMedicalCondition;

        public string SelectedMedicalCondition
        {
            get { return _selectedMedicalCondition; }
            set { _selectedMedicalCondition = value; OnPropertyChanged(nameof(SelectedMedicalCondition)); }
        }

        private ObservableCollection<string> _medicalHistory;

        public ObservableCollection<string> MedicalHistory
        {
            get { return _medicalHistory; }
            set { _medicalHistory = value; OnPropertyChanged(nameof(MedicalHistory)); }
        }

        private ObservableCollection<string> _allergies;

        public ObservableCollection<string> Allergies
        {
            get { return _allergies; }
            set { _allergies = value; OnPropertyChanged(nameof(Allergies)); }
        }


        public ICommand AddAllergyCommand { get; set; }
        public ICommand DeleteAllergyCommand { get; set; }
        public ICommand UpdateAllergyCommand { get; set; }
        public ICommand AddMedicalConditionCommand { get; set; }
        public ICommand UpdateMedicalConditionCommand { get; set; }
        public ICommand DeleteMedicalConditionCommand { get; set; }
        public ICommand ChangeHeightCommand { get; set; }
        public ICommand ChangeWeightCommand { get; set; }


        public MedicalRecordPageViewModel(Patient patient)
        {
            Patient = patient;
            Weight = Patient.MedicalRecord.Weight.ToString();
            Height = Patient.MedicalRecord.Height.ToString();
            Allergies = new ObservableCollection<string>(patient.MedicalRecord.Allergies);
            MedicalHistory = new ObservableCollection<string>(patient.MedicalRecord.MedicalHistory);

            AddAllergyCommand = new RelayCommand(AddAllergyButton_Click);
            DeleteAllergyCommand = new RelayCommand(DeleteAllergyButton_Click);
            UpdateAllergyCommand = new RelayCommand(UpdateAllergyButton_Click);

            AddMedicalConditionCommand = new RelayCommand(AddMedicalConditionButton_Click);
            DeleteMedicalConditionCommand = new RelayCommand(DeleteMedicalConditionButton_Click);
            UpdateMedicalConditionCommand = new RelayCommand(UpdateMedicalConditionButton_Click);

            ChangeHeightCommand = new RelayCommand(ChangeHeightButton_Click);
            ChangeWeightCommand = new RelayCommand(ChangeWeightButton_Click);
        }

        private void AddAllergyButton_Click()
        {
            addHealthCondition(HealthConditionType.Allergy);
        }

        private void AddMedicalConditionButton_Click()
        {
            addHealthCondition(HealthConditionType.MedicalCondition);
        }

        private void UpdateAllergyButton_Click()
        {
            updateHealthCondition(HealthConditionType.Allergy);
        }

        private void UpdateMedicalConditionButton_Click()
        {
            updateHealthCondition(HealthConditionType.MedicalCondition);
        }

        private void DeleteMedicalConditionButton_Click()
        {
            deleteHealthCondition(HealthConditionType.MedicalCondition);
        }

        private void DeleteAllergyButton_Click()
        {
            deleteHealthCondition(HealthConditionType.Allergy);
        }

        private void ChangeWeightButton_Click()
        {
            changePhysicalCharacteristic(false);
        }

        private void ChangeHeightButton_Click()
        {
            changePhysicalCharacteristic(true);
        }

        private void addHealthCondition(HealthConditionType conditionType)
        {
            Action<string> medicalRecordOperation = conditionType == HealthConditionType.Allergy ? Patient.MedicalRecord.AddAllergy : Patient.MedicalRecord.AddMedicalConidition;
            var healthConditionCollection = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

            string conditionToAdd = Interaction.InputBox($"Insert {conditionType}: ", $"Add {conditionType}", "");
            try
            {
                medicalRecordOperation(conditionToAdd);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(Patient);
            refreshHealthConditionCollection(conditionType);
        }

        private void updateHealthCondition(HealthConditionType conditionType)
        {
            Action<string, string> medicalRecordOperation = conditionType == HealthConditionType.Allergy ? Patient.MedicalRecord.UpdateAllergy : Patient.MedicalRecord.UpdateMedicalCondition;
            var selectedHealthCondition = conditionType == HealthConditionType.Allergy ? SelectedAllergy : SelectedMedicalCondition;
            var healthConditionCollection = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

            string? selectedCondition = (string)selectedHealthCondition;
            if (selectedCondition == null)
            {
                MessageBox.Show("You must select condition in order to update it");
                return;
            }
            string updatedCondition = Interaction.InputBox($"Update '{selectedCondition}' name: ", $"Update {conditionType}", "");
            try
            {
                medicalRecordOperation(selectedCondition, updatedCondition);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(Patient);
            refreshHealthConditionCollection(conditionType);
            
        }

        private void deleteHealthCondition(HealthConditionType conditionType)
        {
            Action<string> medicalRecordOperation = conditionType == HealthConditionType.Allergy ? Patient.MedicalRecord.DeleteAllergy : Patient.MedicalRecord.DeleteMedicalCondition;
            var selectedHealthCondition = conditionType == HealthConditionType.Allergy ? SelectedAllergy : SelectedMedicalCondition;
            var healthConditionCollection = conditionType == HealthConditionType.Allergy ? Allergies : MedicalHistory;

            string? selectedCondition = (string)selectedHealthCondition;
            if (selectedCondition == null)
            {
                MessageBox.Show($"You must select {conditionType} in order to delete it");
                return;
            }
            try
            {
                medicalRecordOperation(selectedCondition);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(Patient);
            refreshHealthConditionCollection(conditionType);
        }

        private void changePhysicalCharacteristic(bool isHeight)
        {
            Action<int> medicalRecordOperation = isHeight ? Patient.MedicalRecord.ChangeHeight : Patient.MedicalRecord.ChangeWeight;
            var sizeToChangeStr = isHeight ? Height : Weight;

            int sizeToChange = Int32.Parse(sizeToChangeStr);
            try
            {
                medicalRecordOperation(sizeToChange);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _doctorCoordinator.UpdatePatient(Patient);
            MessageBox.Show("Succeed");
        }

        private void refreshHealthConditionCollection(HealthConditionType conditionType)
        {
            if (conditionType == HealthConditionType.Allergy)
                Allergies = new ObservableCollection<string>(Patient.MedicalRecord.Allergies);
            else
                MedicalHistory = new ObservableCollection<string>(Patient.MedicalRecord.MedicalHistory);
        }
    }
}
