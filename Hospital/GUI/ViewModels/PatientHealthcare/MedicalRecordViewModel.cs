using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.Core.Workers.Services;
using Microsoft.VisualBasic;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class MedicalRecordViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();
    private readonly PatientService _patientService = new();

    private ObservableCollection<string> _allergies;

    private string _height;

    private ObservableCollection<string> _medicalHistory;

    private Patient _patient;

    private string? _selectedAllergy;

    private string? _selectedMedicalCondition;

    private string _weight;


    public MedicalRecordViewModel(Patient patient)
    {
        Patient = patient;
        Weight = Patient.MedicalRecord.Weight.ToString();
        Height = Patient.MedicalRecord.Height.ToString();
        Allergies = new ObservableCollection<string>(patient.MedicalRecord.Allergies.Conditions);
        MedicalHistory = new ObservableCollection<string>(patient.MedicalRecord.MedicalHistory.Conditions);

        AddAllergyCommand = new RelayCommand(AddAllergy);
        DeleteAllergyCommand = new RelayCommand(DeleteAllergy);
        UpdateAllergyCommand = new RelayCommand(UpdateAllergy);

        AddMedicalConditionCommand = new RelayCommand(AddMedicalCondition);
        DeleteMedicalConditionCommand = new RelayCommand(DeleteMedicalCondition);
        UpdateMedicalConditionCommand = new RelayCommand(UpdateMedicalCondition);

        ChangeHeightCommand = new RelayCommand(ChangeHeight);
        ChangeWeightCommand = new RelayCommand(ChangeWeight);
    }

    public Patient Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            OnPropertyChanged(nameof(Patient));
        }
    }

    public string Height //Maybe rename it
    {
        get => _height;
        set
        {
            _height = value;
            OnPropertyChanged(nameof(Height));
        }
    }

    public string Weight
    {
        get => _weight;
        set
        {
            _weight = value;
            OnPropertyChanged(nameof(Weight));
        }
    }

    public string? SelectedAllergy
    {
        get => _selectedAllergy;
        set
        {
            _selectedAllergy = value;
            OnPropertyChanged(nameof(SelectedAllergy));
        }
    }

    public string? SelectedMedicalCondition
    {
        get => _selectedMedicalCondition;
        set
        {
            _selectedMedicalCondition = value;
            OnPropertyChanged(nameof(SelectedMedicalCondition));
        }
    }

    public ObservableCollection<string> MedicalHistory
    {
        get => _medicalHistory;
        set
        {
            _medicalHistory = value;
            OnPropertyChanged(nameof(MedicalHistory));
        }
    }

    public ObservableCollection<string> Allergies
    {
        get => _allergies;
        set
        {
            _allergies = value;
            OnPropertyChanged(nameof(Allergies));
        }
    }


    public ICommand AddAllergyCommand { get; set; }
    public ICommand DeleteAllergyCommand { get; set; }
    public ICommand UpdateAllergyCommand { get; set; }
    public ICommand AddMedicalConditionCommand { get; set; }
    public ICommand UpdateMedicalConditionCommand { get; set; }
    public ICommand DeleteMedicalConditionCommand { get; set; }
    public ICommand ChangeHeightCommand { get; set; }
    public ICommand ChangeWeightCommand { get; set; }

    private void AddAllergy()
    {
        AddHealthCondition(HealthConditionType.Allergy);
    }

    private void AddMedicalCondition()
    {
        AddHealthCondition(HealthConditionType.MedicalCondition);
    }

    private void UpdateAllergy()
    {
        UpdateHealthCondition(HealthConditionType.Allergy);
    }

    private void UpdateMedicalCondition()
    {
        UpdateHealthCondition(HealthConditionType.MedicalCondition);
    }

    private void DeleteMedicalCondition()
    {
        DeleteHealthCondition(HealthConditionType.MedicalCondition);
    }

    private void DeleteAllergy()
    {
        DeleteHealthCondition(HealthConditionType.Allergy);
    }

    private void ChangeWeight()
    {
        ChangePhysicalCharacteristic(false);
    }

    private void ChangeHeight()
    {
        ChangePhysicalCharacteristic(true);
    }

    private void AddHealthCondition(HealthConditionType conditionType)
    {
        var healthConditionToChange = conditionType == HealthConditionType.Allergy
            ? Patient.MedicalRecord.Allergies
            : Patient.MedicalRecord.MedicalHistory;

        var conditionToAdd = Interaction.InputBox($"Insert {conditionType}: ", $"Add {conditionType}");
        try
        {
            healthConditionToChange.Add(conditionToAdd);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        _patientService.UpdatePatient(Patient);
        RefreshHealthConditionCollection(conditionType);
    }

    private void UpdateHealthCondition(HealthConditionType conditionType)
    {
        var healthConditionToChange = conditionType == HealthConditionType.Allergy
            ? Patient.MedicalRecord.Allergies
            : Patient.MedicalRecord.MedicalHistory;

        var selectedHealthCondition =
            conditionType == HealthConditionType.Allergy ? SelectedAllergy : SelectedMedicalCondition;

        if (selectedHealthCondition is null)
        {
            MessageBox.Show("You must select condition in order to update it");
            return;
        }

        var updatedHealthCondition =
            Interaction.InputBox($"Update '{selectedHealthCondition}' name: ", $"Update {conditionType}");
        try
        {
            healthConditionToChange.Update(selectedHealthCondition, updatedHealthCondition);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        _patientService.UpdatePatient(Patient);
        RefreshHealthConditionCollection(conditionType);
    }

    private void DeleteHealthCondition(HealthConditionType conditionType)
    {
        var healthConditionToChange = conditionType == HealthConditionType.Allergy
            ? Patient.MedicalRecord.Allergies
            : Patient.MedicalRecord.MedicalHistory;

        var selectedHealthCondition =
            conditionType == HealthConditionType.Allergy ? SelectedAllergy : SelectedMedicalCondition;

        if (selectedHealthCondition is null)
        {
            MessageBox.Show($"You must select {conditionType} in order to delete it");
            return;
        }

        try
        {
            healthConditionToChange.Delete(selectedHealthCondition);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        _patientService.UpdatePatient(Patient);
        RefreshHealthConditionCollection(conditionType);
    }

    private void ChangePhysicalCharacteristic(bool isHeight)
    {
        Action<int> medicalRecordOperation =
            isHeight ? Patient.MedicalRecord.ChangeHeight : Patient.MedicalRecord.ChangeWeight;
        var physicalCharacteristicString = isHeight ? Height : Weight;

        var sizeToChange = int.Parse(physicalCharacteristicString);
        try
        {
            medicalRecordOperation(sizeToChange);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        _patientService.UpdatePatient(Patient);
        MessageBox.Show("Succeed");
    }

    private void RefreshHealthConditionCollection(HealthConditionType conditionType)
    {
        if (conditionType == HealthConditionType.Allergy)
            Allergies = new ObservableCollection<string>(Patient.MedicalRecord.Allergies.Conditions);
        else
            MedicalHistory = new ObservableCollection<string>(Patient.MedicalRecord.MedicalHistory.Conditions);
    }
}