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
using System.Windows.Shapes;
using Hospital.Models.Manager;
using Hospital.ViewModels;
using Hospital.ViewModels.Manager;

namespace Hospital.Views.Manager
{
    /// <summary>
    /// Interaction logic for MergeRooms.xaml
    /// </summary>
    public partial class MergeRooms : Window, IClosable
    {
        public MergeRooms(List<Room> ToMerge)
        {
            InitializeComponent();
            this.DataContext = new MergeRoomsViewModel(ToMerge);
        }
    }
}
