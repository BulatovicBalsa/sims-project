using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hospital.DTOs;

namespace Hospital.Views.Nurse.Medication
{
    /// <summary>
    /// Interaction logic for MedicationManagementView.xaml
    /// </summary>
    public partial class MedicationManagementView : UserControl
    {
        public MedicationManagementView()
        {
            InitializeComponent();

            MedicationOrderQuantitiesDataGrid.Loaded += (sender, args) =>
            {
                foreach (var item in MedicationOrderQuantitiesDataGrid.ItemContainerGenerator.Items.Cast<MedicationOrderQuantityDto>())
                {
                    if (item.Stock != 0) continue;

                    var row = MedicationOrderQuantitiesDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                    if (row == null)
                    {
                        MedicationOrderQuantitiesDataGrid.ScrollIntoView(item);
                        row = MedicationOrderQuantitiesDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    }

                    row.Background = Brushes.Red;
                }
            };
        }
    }
}
