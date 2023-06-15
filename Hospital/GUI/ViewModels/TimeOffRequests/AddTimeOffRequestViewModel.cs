using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Exceptions;
using Hospital.TimeOffRequests.Models;
using Hospital.TimeOffRequests.Services;
using Hospital.Workers.Models;

namespace Hospital.GUI.ViewModels.TimeOffRequests;

public class AddTimeOffRequestViewModel : ViewModelBase
{
    private readonly Doctor _doctor;
    private readonly DoctorTimeOffRequestService _requestService = new();

    private string? _reason;

    private DateTime? _selectedEnd;

    private DateTime? _selectedStart;

    public AddTimeOffRequestViewModel(Doctor doctor)
    {
        _doctor = doctor;
        _selectedStart = DateTime.Today.AddDays(2);
        _selectedEnd = SelectedStart;
        AddRequestCommand = new RelayCommand<Window>(AddRequest);
    }

    public DateTime? SelectedStart
    {
        get => _selectedStart;
        set
        {
            _selectedStart = value;
            OnPropertyChanged(nameof(SelectedStart));
        }
    }

    public DateTime? SelectedEnd
    {
        get => _selectedEnd;
        set
        {
            _selectedEnd = value;
            OnPropertyChanged(nameof(SelectedEnd));
        }
    }

    public string? Reason
    {
        get => _reason;
        set
        {
            _reason = value;
            OnPropertyChanged(nameof(Reason));
        }
    }

    public ICommand AddRequestCommand { get; set; }

    private void AddRequest(Window window)
    {
        var request = TryGetTimeOffRequest();
        if (request is null) return;

        _requestService.Add(request!);
        MessageBox.Show("Succeed");
        window.DialogResult = true;
    }

    private DoctorTimeOffRequest? TryGetTimeOffRequest()
    {
        DoctorTimeOffRequest? request = null;
        if (SelectedStart is null || SelectedEnd is null)
        {
            MessageBox.Show("You must enter Start and End Date for Time Off Request", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return request;
        }

        try
        {
            request = new DoctorTimeOffRequest(_doctor.Id, Reason, SelectedStart.GetValueOrDefault(),
                SelectedEnd.GetValueOrDefault());
        }
        catch (Exception e)
        {
            if (e is InvalidOperationException or UndefinedTimeOffReasonException)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return request;
            }
        }

        return request;
    }
}