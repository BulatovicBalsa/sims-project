using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.Services;
using Hospital.ViewModels.Librarian.Patients;
using Hospital.Views.Librarian.PatientAdmission;
using Hospital.Views.Librarian.Patients;

namespace Hospital.ViewModels.Librarian.PatientAdmission;

public class PatientAdmissionViewModel : ViewModelBase
{
    private readonly ExaminationService _examinationService;
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;
    private bool _patientCreated = false;

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
            {
                ExecuteStartAdmissionCommand(new Examination(null, SelectedPatient, false, DateTime.MinValue, null));
            }
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
