﻿using System;
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
using Hospital.ViewModels;

namespace Hospital.Views.Manager
{
    /// <summary>
    /// Interaction logic for AddTransfer.xaml
    /// </summary>
    public partial class AddTransfer : Window, IClosable
    {
        public AddTransfer()
        {
            InitializeComponent();
        }
    }
}
