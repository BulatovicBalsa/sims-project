using System;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Hospital.ViewModels.Nurse.Referrals;

public class PatientReferralsViewModel : ViewModelBase
{
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;

    public PatientReferralsViewModel()
    {
        _patientRepository = new PatientRepository();
        _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());
        _selectedPatient = null;

        UseReferralCommand = new ViewModelCommand(ExecuteUseReferralCommand, CanExecuteUseReferralCommand);
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

    public ICommand UseReferralCommand { get; }

    private void ExecuteUseReferralCommand(object obj)
    {
        throw new NotImplementedException();
    }

    private bool CanExecuteUseReferralCommand(object obj)
    {
        return SelectedPatient != null;
    }

}

