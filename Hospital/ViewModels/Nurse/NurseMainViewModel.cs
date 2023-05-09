using System.Windows.Input;
using Hospital.ViewModels.Nurse.PatientAdmission;
using Hospital.ViewModels.Nurse.Patients;
using Hospital.ViewModels.Nurse.UrgentExaminations;

namespace Hospital.ViewModels.Nurse;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    public NurseMainViewModel()
    {
        ShowPatientsViewCommand = new ViewModelCommand(ExecuteShowPatientsViewCommand);
        ShowPatientAdmissionViewCommand = new ViewModelCommand(ExecuteShowPatientAdmissionViewCommand);
        ShowUrgentExaminationsViewCommand = new ViewModelCommand(ExecuteShowUrgentExaminationsViewCommand);

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
    public ICommand ShowUrgentExaminationsViewCommand { get; }

    private void ExecuteShowPatientsViewCommand(object? obj)
    {
        CurrentChildView = new PatientGridViewModel();
    }

    private void ExecuteShowPatientAdmissionViewCommand(object obj)
    {
        CurrentChildView = new PatientAdmissionViewModel();
    }

    private void ExecuteShowUrgentExaminationsViewCommand(object obj)
    {
        CurrentChildView = new UrgentExaminationsViewModel();
    }
}
