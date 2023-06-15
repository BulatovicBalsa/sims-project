using System.Windows;
using Hospital.GUI.ViewModels;
using Hospital.GUI.ViewModels.PhysicalAssets;

namespace Hospital.GUI.Views.PhysicalAssets;

public partial class AddOrder : Window, IClosable
{
    public AddOrder()
    {
        InitializeComponent();
        DataContext = new AddOrderViewModel();
    }
}