using GalaSoft.MvvmLight.Command;
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
    public class HospitalFeedbackViewModel : ViewModelBase
    {
        private readonly FeedbackService _feedbackService;

        public HospitalFeedbackViewModel()
        {
            _feedbackService = new FeedbackService();
            SubmitCommand = new RelayCommand(SubmitFeedback);
        }

        public int Rating { get; set; }
        public int RecommendationRating { get; set; }
        public int ServiceQualityRating { get; set; }
        public int CleanlinessRating { get; set; }
        public int SatisfactionRating { get; set; }
        public string Comment { get; set; }

        public ICommand SubmitCommand { get; }

        private void SubmitFeedback()
        {
            HospitalFeedback feedback = new HospitalFeedback(Rating, RecommendationRating, Comment, ServiceQualityRating, CleanlinessRating, SatisfactionRating);
            _feedbackService.SubmitHospitalFeedback(feedback);
            MessageBox.Show("Feedback submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
