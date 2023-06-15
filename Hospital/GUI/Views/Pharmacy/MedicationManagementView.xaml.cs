using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Hospital.DTOs;

namespace Hospital.GUI.Views.Pharmacy;

/// <summary>
///     Interaction logic for MedicationManagementView.xaml
/// </summary>
public partial class MedicationManagementView : UserControl
{
    public MedicationManagementView()
    {
        InitializeComponent();

        MedicationOrderQuantitiesDataGrid.Loaded += (sender, args) =>
        {
            foreach (var item in MedicationOrderQuantitiesDataGrid.ItemContainerGenerator.Items
                         .Cast<MedicationOrderQuantityDto>())
            {
                if (item.Stock != 0) continue;

                var row =
                    MedicationOrderQuantitiesDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (row == null)
                {
                    MedicationOrderQuantitiesDataGrid.ScrollIntoView(item);
                    row = MedicationOrderQuantitiesDataGrid.ItemContainerGenerator.ContainerFromItem(item) as
                        DataGridRow;
                }

                row.Background = Brushes.Red;
            }
        };
    }
}