using System.Windows;

namespace Hospital.Views.Manager;

public partial class ManagerView : Window
{
    public ManagerView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var equipmentView = new EquipmentView();
        equipmentView.Show();
    }
}