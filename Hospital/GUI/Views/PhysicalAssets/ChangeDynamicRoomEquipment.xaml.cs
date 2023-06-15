using System.Windows;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.GUI.ViewModels.PhysicalAssets;

namespace Hospital.GUI.Views.PhysicalAssets;

/// <summary>
///     Interaction logic for ChangeDynamicalRoomEquipment.xaml
/// </summary>
public partial class ChangeDynamicRoomEquipment : Window
{
    public ChangeDynamicRoomEquipment(Room room)
    {
        DataContext = new RoomInventoryViewModel(room);
        InitializeComponent();
    }
}