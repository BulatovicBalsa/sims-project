using System.Collections.Generic;
using System.Windows;
using Hospital.GUI.ViewModels;
using Hospital.GUI.ViewModels.PhysicalAssets;
using Hospital.PhysicalAssets.Models;

namespace Hospital.GUI.Views.PhysicalAssets;

/// <summary>
///     Interaction logic for MergeRooms.xaml
/// </summary>
public partial class MergeRooms : Window, IClosable
{
    public MergeRooms(List<Room> ToMerge)
    {
        InitializeComponent();
        DataContext = new MergeRoomsViewModel(ToMerge);
    }
}