using System.Windows;
using Hospital.Charting;
using Hospital.ViewModels.Manager;

namespace Hospital.Views.Manager;

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