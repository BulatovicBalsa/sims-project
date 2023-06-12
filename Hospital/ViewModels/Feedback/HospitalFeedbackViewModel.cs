﻿using GalaSoft.MvvmLight.Command;
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
        private readonly Window _view;

        public HospitalFeedbackViewModel(Window window)
        {
            _feedbackService = new FeedbackService();
            SubmitCommand = new RelayCommand(SubmitFeedback);
            _view = window;
            OverallRating = 1;
            RecommendationRating = 1;
            ServiceQualityRating = 1;
            CleanlinessRating = 1;
            SatisfactionRating = 1;
            Comment="";
        }

        public int OverallRating { get; set; }
        public int RecommendationRating { get; set; }
        public int ServiceQualityRating { get; set; }
        public int CleanlinessRating { get; set; }
        public int SatisfactionRating { get; set; }
        public string Comment { get; set; }

        public ICommand SubmitCommand { get; }

        private void SubmitFeedback()
        {
            HospitalFeedback feedback = new HospitalFeedback(OverallRating, RecommendationRating, Comment, ServiceQualityRating, CleanlinessRating, SatisfactionRating);
            _feedbackService.SubmitHospitalFeedback(feedback);
            MessageBox.Show("Feedback submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _view.Close();
        }
    }
}