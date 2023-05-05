using System.Windows.Input;
using Hospital.ViewModels.Nurse.Patients;

namespace Hospital.ViewModels.Nurse;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    public NurseMainViewModel()
    {
        ShowPatientsViewCommand = new ViewModelCommand(ExecuteShowPatientsViewCommand);

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

    private void ExecuteShowPatientsViewCommand(object obj)
    {
        CurrentChildView = new PatientGridViewModel();
    }
}
