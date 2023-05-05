using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.ViewModels.Nurse.Patients;
using Hospital.Views.Nurse.Patients;

namespace Hospital.ViewModels.Nurse.PatientAdmission;

public class PatientAdmissionViewModel : ViewModelBase
{
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;

    public PatientAdmissionViewModel()
    {
        _patientRepository = new PatientRepository();
        _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());

        _patientRepository.PatientAdded += patient => { _patients.Add(patient); };

        AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand);
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
        var addPatientDialog = new AddPatientView
        {
            DataContext = new AddUpdatePatientViewModel(_patientRepository)
        };

        addPatientDialog.ShowDialog();

        //ExecuteStartAdmissionCommand();
    }

    private void ExecuteStartAdmissionCommand(object obj)
    {
        throw new NotImplementedException();
    }

    private bool IsPatientSelected(object obj)
    {
        return _selectedPatient != null;
    }
}
