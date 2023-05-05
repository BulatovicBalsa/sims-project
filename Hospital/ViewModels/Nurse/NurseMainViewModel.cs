using System.Windows.Input;
using Hospital.ViewModels.Nurse.PatientAdmission;
using Hospital.ViewModels.Nurse.Patients;

namespace Hospital.ViewModels.Nurse;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    public NurseMainViewModel()
    {
        ShowPatientsViewCommand = new ViewModelCommand(ExecuteShowPatientsViewCommand);
        ShowPatientAdmissionViewCommand = new ViewModelCommand(ExecuteShowPatientAdmissionViewCommand);

        ExecuteShowPatientsViewCommand(null);
    }

    public ViewModelBase CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged(nameof(CurrentChildView));
        }
    }
    public ICommand ShowPatientsViewCommand { get; }
    public ICommand ShowPatientAdmissionViewCommand { get; }

    private void ExecuteShowPatientsViewCommand(object obj)
    {
        CurrentChildView = new PatientGridViewModel();
    }

    private void ExecuteShowPatientAdmissionViewCommand(object obj)
    {
        CurrentChildView = new PatientAdmissionViewModel();
    }
}
