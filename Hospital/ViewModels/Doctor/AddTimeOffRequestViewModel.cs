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

namespace Hospital.ViewModels
{
    public class AddTimeOffRequestViewModel: ViewModelBase
    {
        private readonly DoctorTimeOffRequestService _requestService = new();
        private Doctor _doctor;
        
        private DateTime? _selectedStart;
        public DateTime? SelectedStart
        {
            get => _selectedStart;
            set
            {
                _selectedStart = value;
                OnPropertyChanged(nameof(SelectedStart));
            }
        }

        private DateTime? _selectedEnd;
        public DateTime? SelectedEnd
        {
            get => _selectedEnd;
            set
            {
                _selectedEnd = value;
                OnPropertyChanged(nameof(SelectedEnd));
            }
        }

        private string? _reason;
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

        public AddTimeOffRequestViewModel(Doctor doctor)
        {
            _doctor = doctor;
            _selectedStart = DateTime.Today.AddDays(2);
            _selectedEnd = SelectedStart;
            AddRequestCommand = new RelayCommand<Window>(AddRequest);
        }

        private void AddRequest(Window window)
        {
            DoctorTimeOffRequest request;
            if (SelectedStart is null || SelectedEnd is null)
            {
                MessageBox.Show("You must enter Start and End Date for Time Off Request", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                request = new DoctorTimeOffRequest(_doctor.Id, Reason, SelectedStart.GetValueOrDefault(),
                    SelectedEnd.GetValueOrDefault());
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is UndefinedTimeOffReasonException)
                {
                    MessageBox.Show("You must enter Start and End Date for Time Off Request", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            _requestService.Add(request);
        }
    }
}
