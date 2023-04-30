﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.Views.Nurse.Patients;

namespace Hospital.ViewModels.Nurse.Patients;

public class PatientGridViewModel : ViewModelBase
{
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;

    public PatientGridViewModel()
    {
        _patientRepository = new PatientRepository();
        _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());

        _patientRepository.PatientAdded += (patient) =>
        {
            _patients.Add(patient);
        };

        _patientRepository.PatientUpdated += (patient) =>
        {
            _patients.Remove(patient);
            _patients.Add(patient);
        };

        AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand);
        UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand, IsPatientSelected);
        DeletePatientCommand = new ViewModelCommand(ExecuteDeletePatientCommand, IsPatientSelected);
        ShowMedicalRecordCommand = new ViewModelCommand(ExecuteShowMedicalRecordCommand, IsPatientSelected);
    }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set
        {
            _patients = value;
            OnPropertyChanged(nameof(Patients));
        }
    }

    public ICommand AddPatientCommand { get; }
    public ICommand UpdatePatientCommand { get; }
    public ICommand DeletePatientCommand { get; }
    public ICommand ShowMedicalRecordCommand { get; }

    private void ExecuteAddPatientCommand (object obj)
    {
        var addPatientDialog = new AddPatientView
        {
            DataContext = new AddUpdatePatientViewModel(_patientRepository)
        };

        addPatientDialog.ShowDialog();
    }

    private void ExecuteUpdatePatientCommand(object obj)
    {
        var updatePatientDialog = new UpdatePatientView
        {
            DataContext = new AddUpdatePatientViewModel(_patientRepository, _selectedPatient)
        };

        updatePatientDialog.ShowDialog();
    }

    private void ExecuteDeletePatientCommand(object obj)
    {
        _patientRepository.Delete(SelectedPatient);
        _patients.Remove(SelectedPatient);
        SelectedPatient = null;
    }

    private void ExecuteShowMedicalRecordCommand(object obj)
    {
        var medicalRecordDialog = new MedicalRecordView
        {
            DataContext = new MedicalRecordViewModel(_selectedPatient.MedicalRecord)
        };

        medicalRecordDialog.ShowDialog();
    }

    private bool IsPatientSelected(object obj)
    {
        return _selectedPatient != null;
    }
}