using System;
using Hospital.Notifications.Models;
using Hospital.Notifications.Services;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.ViewModels.Notifications;

public class PatientNotificationViewModel : ViewModelBase
{
    private string _message;
    private DateTime _selectedDateTime;

    public PatientNotificationViewModel(Patient patient)
    {
        Patient = patient;
        _notificationService = new NotificationService();
        _message = "";
        _selectedDateTime = DateTime.Now;
    }

    private NotificationService _notificationService { get; }

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public DateTime SelectedDateTime
    {
        get => _selectedDateTime;
        set
        {
            _selectedDateTime = value;
            OnPropertyChanged(nameof(SelectedDateTime));
        }
    }

    public Patient Patient { get; }

    internal void CreateNotification()
    {
        var notification = new Notification(Patient.Id, Message);
        notification.NotifyTime = SelectedDateTime;
        _notificationService.Send(notification);
    }
}