﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Hospital.Charting;
using Hospital.Repositories.Feedback;
using Hospital.ViewModels.Manager;
using ScottPlot;

namespace Hospital.Views.Manager;

public partial class ManagerView : Window
{
    public ManagerView()
    {
        InitializeComponent();
        DataContext = new ManagerViewModel(new RatingFrequencyPiePlotter(HospitalFeedbackFrequencyPlot));
        PlotAverageHospitalFeedback();
    }

    private void PlotAverageHospitalFeedback()
    {
        var averageFeedbackByArea = HospitalFeedbackRepository.Instance.GetAverageGrades();
        double[] values =
        {
            averageFeedbackByArea.OverallRating, averageFeedbackByArea.RecommendationRating,
            averageFeedbackByArea.ServiceQuality, averageFeedbackByArea.PatientSatisfactionRating,
            averageFeedbackByArea.CleanlinessRating
        };
        string[] labels =
            { "Overall rating", "Recommendation rating", "Service quality", "Patient satisfaction", "Cleanliness" };
        var bar = WpfPlot.Plot.AddBar(values);
        WpfPlot.Plot.YTicks(labels);
        WpfPlot.Plot.SetAxisLimitsX(xMin: 0, xMax: 5.9);
        bar.Orientation = Orientation.Horizontal;
        bar.ShowValuesAboveBars = true;
        bar.ValueFormatter = d => Math.Round(d, 2).ToString(CultureInfo.InvariantCulture);
        WpfPlot.Plot.Title("Average grade by area");
        WpfPlot.Refresh();
        WpfPlot.Plot.AxisAutoY();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var equipmentView = new EquipmentView();
        equipmentView.Show();
    }
}