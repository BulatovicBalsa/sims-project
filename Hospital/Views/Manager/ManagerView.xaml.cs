using System;
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
        DataContext = new ManagerViewModel(new RatingFrequencyPiePlotter(HospitalFeedbackFrequencyPlot), new RatingAreaBarPlot(AverageHospitalFeedbackRatingByAreaWpfPlot));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var equipmentView = new EquipmentView();
        equipmentView.Show();
    }
}