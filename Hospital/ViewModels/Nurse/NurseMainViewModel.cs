using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.ViewModels.Nurse.Patients;

namespace Hospital.ViewModels.Nurse;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    private string _errorMessage;

    public NurseMainViewModel()
    {
        ShowPatientsView = new ViewModelCommand(ExecuteShowPatientsViewCommand);

        ExecuteShowPatientsViewCommand(null);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
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
    public ICommand ShowPatientsView { get; }

    private void ExecuteShowPatientsViewCommand(object obj)
    {
        CurrentChildView = new PatientGridViewModel();
    }
}
