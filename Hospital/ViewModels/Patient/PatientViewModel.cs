using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Patient;
using System.Runtime.CompilerServices;
using Hospital.Models.Examination;
using Hospital.Services;
using System.Windows;
using Hospital.Models;
using System.Windows.Threading;

namespace Hospital.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        private const int NotificationIntervalMinutes = 1;

        private ObservableCollection<Examination> _examinations;
        private int _notificationTime;
        private readonly ExaminationService _examinationService;
        private readonly NotificationService _notificationService;
        private readonly PatientService _patientService;
        private Patient _patient;
        private DispatcherTimer _notificationTimer;


        public ObservableCollection<Examination> Examinations
        {
            get { return _examinations; }
            set
            {
                _examinations = value;
                OnPropertyChanged();
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int NotificationTime
        {
            get { return _notificationTime; }
            set
            {
                _notificationTime = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PatientViewModel(Patient patient)
        {
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            _examinationService = new ExaminationService();
            _notificationService = new NotificationService();
            _patientService = new PatientService();
            _patient = patient;
            _notificationTime = patient.NotificationTime;

            LoadExaminations();
            DisplayPatientNotifications(this,EventArgs.Empty);
            StartNotificationTimer();
        }

        public void LoadExaminations()
        {
            var examinations = _examinationService.GetAllExaminations(_patient);

            Examinations = new ObservableCollection<Examination>(examinations);
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void StartNotificationTimer()
        {
            _notificationTimer = new DispatcherTimer();
            _notificationTimer.Tick += new EventHandler(DisplayPatientNotifications);
            _notificationTimer.Interval = new TimeSpan(0, NotificationIntervalMinutes, 0);
            _notificationTimer.Start();
        }

        public void AddExamination(Examination examination)
        {
            _examinationService.AddExamination(examination, true);
            Examinations.Add(examination);
        }

        public void UpdateExamination(Examination examination)
        {
            _examinationService.UpdateExamination(examination, true);
        }

        public void DeleteExamination(Examination examination)
        {
            _examinationService.DeleteExamination(examination, true);
            Examinations.Remove(examination);
        }

        public void RefreshExaminations()
        {
            Examinations = new ObservableCollection<Examination>(_examinationService.GetAllExaminations(_patient));
        }
        private void DisplayPatientNotifications(object sender, EventArgs e)
        {
            var notifications = _notificationService.GetAllUnsent(_patient.Id)
             .Where(notification => notification.ShouldBeSent())
             .ToList();

            notifications.ForEach(async notification =>
            {
                MessageBox.Show(notification.Message,"Notification");
                _notificationService.MarkSent(notification);
                //await Task.Delay(10); // Delay to allow UI updates
            });
        }
        internal void SaveNotificationTime(int notificationTime)
        {
            NotificationTime = notificationTime;
            _patient.NotificationTime = notificationTime;
            _patientService.UpdatePatient(_patient);
        }
    }
}
