﻿using Hospital.Models.Manager;
using Hospital.ViewModels;
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

namespace Hospital.Views
{
    /// <summary>
    /// Interaction logic for ChangeDynamicalRoomEquipment.xaml
    /// </summary>
    public partial class ChangeDynamicalRoomEquipment : Window
    {
        public ChangeDynamicalRoomEquipment(Room room)
        {
            DataContext = new ChangeDynamicalRoomEquipmentViewModel(room);
            InitializeComponent();
        }
    }
}
