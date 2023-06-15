using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class ExaminationViewModel : INotifyPropertyChanged
{
    private Examination _examination;
    private bool _isUpdate;
    private Patient _patient;
    private readonly PatientViewModel _patientViewModel;
    private IEnumerable<Doctor> _recommendedDoctors;
    private DateTime? _selectedDate;

    public ExaminationViewModel(Patient patient, PatientViewModel patientViewModel,
        Examination examination = null, Doctor doctor = null)
    {
        _patientViewModel = patientViewModel;
        RecommendedDoctors =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetAll();
        Patient = patient;

        if (examination == null)
        {
            Examination = new Examination();
            Examination.Doctor = doctor;
            Examination.Patient = patient;
            SelectedDate = DateTime.Now;
            IsUpdate = false;
        }
        else
        {
            Examination = examination.DeepCopy();
            SelectedDate = Examination.Start;
            IsUpdate = true;
        }

        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    public string SelectedTime
    {
        get => Examination.Start.ToString("HH:mm");
        set
        {
            TimeSpan time;
            if (TimeSpan.TryParse(value, out time) && IsValidDateTime(SelectedDate, time))
            {
                Examination.Start = Examination.Start.Date + time;
                OnPropertyChanged();
            }
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (IsValidDateTime(value, Examination.Start.TimeOfDay))
            {
                _selectedDate = value;
                OnPropertyChanged();

                if (_selectedDate.HasValue)
                    Examination.Start = _selectedDate.Value.Date.Add(Examination.Start.TimeOfDay);
            }
        }
    }

    public Patient Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            OnPropertyChanged();
        }
    }

    public Examination Examination
    {
        get => _examination;
        set
        {
            _examination = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Doctor> RecommendedDoctors
    {
        get => _recommendedDoctors;
        set
        {
            _recommendedDoctors = value;
            OnPropertyChanged();
        }
    }

    public bool IsUpdate
    {
        get => _isUpdate;
        set
        {
            _isUpdate = value;
            OnPropertyChanged();
            Title = IsUpdate ? "Update Examination" : "Add Examination";
        }
    }

    public string Title { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void Save()
    {
        var errorMessage = ValidateExamination();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            MessageBox.Show(errorMessage, "Error");
            return;
        }

        try
        {
            if (IsUpdate)
            {
                _patientViewModel.UpdateExamination(Examination);
                MessageBox.Show("Examination updated successfully", "Success");
            }
            else
            {
                _patientViewModel.AddExamination(Examination);
                MessageBox.Show("Examination added successfully", "Success");
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        _patientViewModel.RefreshExaminations();
        Close();
    }

    private void Cancel()
    {
        Close();
    }

    private void Close()
    {
        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        window?.Close();
    }


    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private bool IsValidDateTime(DateTime? date, TimeSpan time)
    {
        if (!date.HasValue)
            return false;

        var dateTime = date.Value.Date + time;
        return dateTime >= DateTime.Now;
    }

    private string ValidateExamination()
    {
        if (Examination.Start < DateTime.Now) return "Examination can't be in the past";

        if (Examination.Doctor == null) return "Please select doctor";

        if (!IsValidDateTime(SelectedDate, Examination.Start.TimeOfDay)) return "Invalid time input";

        return string.Empty;
    }

    private void HandleException(Exception ex)
    {
        MessageBox.Show(ex.Message, "Error");

        if (ex.Message.Contains("Patient made too many changes in last 30 days") ||
            ex.Message.Contains("Patient made too many examinations in last 30 days"))
        {
            Patient.IsBlocked = true;
            PatientRepository.Instance.Update(Patient);
            MessageBox.Show(
                "This user is now blocked due to too many changes or examinations made in the last 30 days.",
                "User Blocked");
            Application.Current.Shutdown();
        }

        _patientViewModel.RefreshExaminations();
    }
}