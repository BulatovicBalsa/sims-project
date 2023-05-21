using Hospital.Models;
using Hospital.Models.Patient;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class PatientNotificationViewModel : ViewModelBase
    {
        private string _message;
        private DateTime _selectedDateTime;
        private NotificationService _notificationService { get; set; }

        public string Message
        {
            get { return _message; }
            set
            {
                Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public DateTime SelectedDateTime
        {
            get { return _selectedDateTime; }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }

        public Patient Patient { get; }

        public PatientNotificationViewModel(Patient patient)
        {
            Patient = patient;
            _notificationService = new NotificationService();
            _message = "";
        }

        internal void CreateNotification()
        {
            var notification = new Notification(Patient.Id, Message);
            notification.NotifyTime = SelectedDateTime;
            _notificationService.Send(notification);
        }
    }
}
