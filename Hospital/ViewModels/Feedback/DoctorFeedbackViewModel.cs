using GalaSoft.MvvmLight.Command;
using Hospital.Models.Doctor;
using Hospital.Models.Feedback;
using Hospital.Services.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels.Feedback
{
    public class DoctorFeedbackViewModel : ViewModelBase
    {
        private readonly FeedbackService _feedbackService;

        public DoctorFeedbackViewModel(Doctor doctor)
        {
            _feedbackService = new FeedbackService();
            SubmitCommand = new RelayCommand(SubmitFeedback);
            Doctor = doctor;
        }

        public Doctor Doctor { get; set; }
        public int OverallRating { get; set; }
        public int DoctorQualityRating { get; set; }
        public int RecommendationRating { get; set; }
        public string Comment { get; set; }

        public ICommand SubmitCommand { get; }

        private void SubmitFeedback()
        {
            DoctorFeedback feedback = new DoctorFeedback(Doctor.Id, OverallRating, RecommendationRating, Comment, DoctorQualityRating);
            _feedbackService.SubmitDoctorFeedback(feedback);
            MessageBox.Show("Feedback submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
