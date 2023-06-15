using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class VisitingDialogViewModel : ViewModelBase
{
    private readonly VisitService _visitService;
    private string _bloodPressure;
    private string _bodyTemperature;
    private string _observations;
    private readonly string _patientId;

    public VisitingDialogViewModel()
    {
        _visitService = new VisitService();
        _patientId = "";
        _bloodPressure = "";
        _bodyTemperature = "";
        _observations = "";
        FinishVisitCommand = new ViewModelCommand(ExecuteFinishVisitCommand, CanExecuteFinishVisitCommand);
    }

    public VisitingDialogViewModel(string patientId)
    {
        _visitService = new VisitService();
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
            _bodyTemperature = value;
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
        var bloodPressureRx = new Regex(@"^\d+\/\d+$");

        if (!bloodPressureRx.IsMatch(BloodPressure))
        {
            MessageBox.Show("Wrong blood pressure format. It has to be {Number}/{Number}", "Error");
            return;
        }

        if (!float.TryParse(BodyTemperature, out var bodyTemperatureNumber))
        {
            MessageBox.Show("Body temperature has to be a valid floating point number", "Error");
            return;
        }

        var newVisit = new Visit(_patientId, BloodPressure, bodyTemperatureNumber, Observations, DateTime.Now);
        _visitService.Add(newVisit);

        MessageBox.Show("Visit completed successfully", "Success");
        CloseDialog();
    }

    private bool CanExecuteFinishVisitCommand(object obj)
    {
        return !string.IsNullOrEmpty(BloodPressure) && !string.IsNullOrEmpty(BodyTemperature);
    }

    private void CloseDialog()
    {
        Application.Current.Windows[1]?.Close();
    }
}