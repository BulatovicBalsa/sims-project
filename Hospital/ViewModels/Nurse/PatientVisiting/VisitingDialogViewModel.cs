using System;
using System.Windows.Input;

namespace Hospital.ViewModels.Nurse.PatientVisiting;
public class VisitingDialogViewModel : ViewModelBase
{
    private string _patientId;
    private string _bloodPressure;
    private string _bodyTemperature;
    private string _observations;

    public VisitingDialogViewModel()
    {
        _patientId = "";
        _bloodPressure = "";
        _bodyTemperature = "";
        _observations = "";
        FinishVisitCommand = new ViewModelCommand(ExecuteFinishVisitCommand, CanExecuteFinishVisitCommand);
    }

    public VisitingDialogViewModel(string patientId)
    {
        _patientId = patientId;
        _bloodPressure = "";
        _bodyTemperature = "";
        _observations = "";
        FinishVisitCommand = new ViewModelCommand(ExecuteFinishVisitCommand, CanExecuteFinishVisitCommand);
    }

    public string BloodPressure
    {
        get => _bloodPressure;
        set
        {
            _bloodPressure = value;
            OnPropertyChanged(nameof(BloodPressure));
        }
    }

    public string BodyTemperature
    {
        get => _bodyTemperature;
        set
        {
            _bloodPressure = value;
            OnPropertyChanged(nameof(BodyTemperature));
        }
    }

    public string Observations
    {
        get => _observations;
        set
        {
            _observations = value;
            OnPropertyChanged(nameof(Observations));
        }
    }

    public ICommand FinishVisitCommand { get; }

    private void ExecuteFinishVisitCommand(object obj)
    {
        throw new NotImplementedException();
    }

    private bool CanExecuteFinishVisitCommand(object obj)
    {
        return string.IsNullOrEmpty(BloodPressure) && string.IsNullOrEmpty(BodyTemperature);
    }
}
