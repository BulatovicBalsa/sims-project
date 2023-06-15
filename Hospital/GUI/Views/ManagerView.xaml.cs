using System.Windows;
using Hospital.GUI.Charting;
using Hospital.GUI.ViewModels;
using Hospital.GUI.Views.PhysicalAssets;

namespace Hospital.GUI.Views;

public partial class ManagerView : Window
{
    public ManagerView()
    {
        InitializeComponent();
        DataContext = new ManagerViewModel(new RatingFrequencyPiePlot(HospitalFeedbackFrequencyPlot),
            new RatingAreaBarPlot(AverageHospitalFeedbackRatingByAreaWpfPlot),
            new RatingFrequencyPiePlot(DoctorRatingFrequencyWpfPlot),
            new RatingAreaBarPlot(AverageDoctorRatingByAreaWpfPlot));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var equipmentView = new EquipmentView();
        equipmentView.Show();
    }
}