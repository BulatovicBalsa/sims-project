using System.Windows;
using Hospital.GUI.ViewModels;
using Hospital.GUI.ViewModels.PhysicalAssets;
using Hospital.PhysicalAssets.Models;

namespace Hospital.GUI.Views.PhysicalAssets;

public partial class SplitRoom : Window, IClosable
{
    public SplitRoom(Room toSplit)
    {
        InitializeComponent();
        DataContext = new SplitRoomViewModel(toSplit);
    }
}