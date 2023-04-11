using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows.Shapes;
using Hospital.Repositories.Manager;

namespace Hospital.Views.Manager
{
    /// <summary>
    /// Interaction logic for EquipmentView.xaml
    /// </summary>
    public partial class EquipmentView : Window
    {
        public ObservableCollection<Models.Manager.Equipment> Equipment { get; set; }

        public EquipmentView()
        {
            InitializeComponent();
            Equipment = new ObservableCollection<Models.Manager.Equipment>();
            this.DataContext = this;
            
            foreach (var e in new EquipmentRepository().GetAll())
            {
                Equipment.Add(e);
            }
        }
    }
}
