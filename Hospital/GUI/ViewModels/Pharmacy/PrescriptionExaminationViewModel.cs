using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.Core.Scheduling.Services;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Services;

namespace Hospital.GUI.ViewModels.Pharmacy;

public class PrescriptionExaminationViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService;
    private readonly ExaminationService _examinationService;
    private readonly PatientService _patientService;
    private readonly TimeslotService _timeslotService;
    private readonly Doctor _doctor;
    private readonly Patient _patient;
    private ObservableCollection<TimeOnly>? _possibleTimeslots;
    private DateTime? _selectedDate;
    private TimeOnly? _selectedTime;

    public PrescriptionExaminationViewModel()
    {
    }

    public PrescriptionExaminationViewModel(string doctorId, string patientId)
    {
        _patientService = new PatientService();
        _doctorService = new DoctorService();
        _timeslotService = new TimeslotService();
        _examinationService = new ExaminationService();
        _doctor = _doctorService.GetById(doctorId);
        DoctorName = $"{_doctor?.FirstName} {_doctor?.LastName}";
        _patient = _patientService.GetPatientById(patientId);
        _selectedDate = null;
        _selectedTime = null;

        ScheduleExaminationCommand =
            new ViewModelCommand(ExecuteScheduleExaminationCommand, CanExecuteScheduleExaminationCommand);
    }

    public string DoctorName { get; set; }

    public ObservableCollection<TimeOnly>? PossibleTimeslots
    {
        get => _possibleTimeslots;
        set
        {
            _possibleTimeslots = value;
            OnPropertyChanged(nameof(PossibleTimeslots));
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
            if (value != null)
                PossibleTimeslots =
                    new ObservableCollection<TimeOnly>(
                        _timeslotService.GetUpcomingFreeTimeslotsForDate(_doctor, (DateTime)SelectedDate));
        }
    }

    public TimeOnly? SelectedTime
    {
        get => _selectedTime;
        set
        {
            _selectedTime = value;
            OnPropertyChanged(nameof(SelectedTime));
        }
    }

    public ICommand ScheduleExaminationCommand { get; }

    private void ExecuteScheduleExaminationCommand(object obj)
    {
        var examinationStart = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, SelectedDate.Value.Day,
            SelectedTime.Value.Hour, SelectedTime.Value.Minute, 0);
        var newExamination = new Examination(_doctor, _patient, false, examinationStart, null);
        try
        {
            _examinationService.AddExamination(newExamination, false);
        }
        catch (Exception)
        {
            MessageBox.Show("Selected timeslot is already occupied", "Error");
            return;
        }

        MessageBox.Show("Examination successfully created", "Success");
        CloseDialog();
    }

    private bool CanExecuteScheduleExaminationCommand(object obj)
    {
        return SelectedDate != null && SelectedTime != null;
    }

    private void CloseDialog()
    {
        Application.Current.Windows[1]?.Close();
    }
}