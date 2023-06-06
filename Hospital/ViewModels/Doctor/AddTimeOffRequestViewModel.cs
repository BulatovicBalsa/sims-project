using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Requests;
using Hospital.Services.Requests;

namespace Hospital.ViewModels;

public class AddTimeOffRequestViewModel : ViewModelBase
{
    private readonly DoctorTimeOffRequestService _requestService = new();
    private readonly Doctor _doctor;

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
        DoctorTimeOffRequest request = null;
        if (SelectedStart is null || SelectedEnd is null)
        {
            MessageBox.Show("You must enter Start and End Date for Time Off Request", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
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
                return;
            }
        }

        _requestService.Add(request!);
        MessageBox.Show("Succeed");
        window.DialogResult = true;
    }
}