using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.GUI.ViewModels.PatientManagement;
using Hospital.GUI.Views.PatientHealthcare;
using Hospital.GUI.Views.PatientManagement;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class PatientAdmissionViewModel : ViewModelBase
{
    private readonly ExaminationService _examinationService;
    private readonly PatientRepository _patientRepository;
    private bool _patientCreated;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;

    public PatientAdmissionViewModel()
    {
        _examinationService = new ExaminationService();
        _patientRepository = PatientRepository.Instance;
        _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());

        _patientRepository.PatientAdded += patient =>
        {
            _patients.Add(patient);
            _selectedPatient = patient;
            _patientCreated = true;
        };

        _patientRepository.PatientUpdated += patient =>
        {
            _patients.Remove(patient);
            _patients.Add(patient);
        };

        AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand);
        StartAdmissionCommand = new ViewModelCommand(ExecuteStartAdmissionCommand, IsPatientSelected);
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
    public ICommand StartAdmissionCommand { get; }

    private void ExecuteAddPatientCommand(object obj)
    {
        var viewModel = new AddUpdatePatientViewModel(_patientRepository);
        viewModel.DialogClosed += () =>
        {
            if (_patientCreated)
                ExecuteStartAdmissionCommand(new Examination(null, SelectedPatient, false, DateTime.MinValue, null));
        };

        OpenAddPatientDialog(viewModel);
    }

    private void OpenAddPatientDialog(AddUpdatePatientViewModel viewModel)
    {
        var addPatientDialog = new AddPatientView
        {
            DataContext = viewModel
        };

        addPatientDialog.ShowDialog();
    }

    private void ExecuteStartAdmissionCommand(object? obj)
    {
        Examination? admissibleExamination = ProcessAdmissibleExamination(obj as Examination);
        if (admissibleExamination == null)
            return;

        OpenPatientAdmissionDialog(admissibleExamination);
    }

    private Examination? ProcessAdmissibleExamination(Examination? admissibleExamination)
    {
        if (admissibleExamination != null) return admissibleExamination;

        admissibleExamination = _examinationService.GetAdmissibleExamination(SelectedPatient);
        if (admissibleExamination != null) return admissibleExamination;

        MessageBox.Show("Selected patient does not have an examination in the next 15 minutes", "Error");
        return null;
    }

    private void OpenPatientAdmissionDialog(Examination admissibleExamination)
    {
        var patientAdmissionDialog = new AdmissionDialogView
        {
            DataContext = new AdmissionDialogViewModel(_patientRepository, SelectedPatient, admissibleExamination)
        };

        patientAdmissionDialog.ShowDialog();
    }

    private bool IsPatientSelected(object obj)
    {
        return _selectedPatient != null;
    }
}