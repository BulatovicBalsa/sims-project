using System.Windows;
using Hospital.GUI.ViewModels;
using Hospital.GUI.ViewModels.PhysicalAssets;

namespace Hospital.GUI.Views.PhysicalAssets;

/// <summary>
///     Interaction logic for AddRenovation.xaml
/// </summary>
public partial class AddRenovation : Window, IClosable
{
    public AddRenovation()
    {
        InitializeComponent();
        DataContext = new AddSimpleRenovationViewModel();
    }
}