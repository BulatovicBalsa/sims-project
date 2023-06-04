﻿using Hospital.DTOs;
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
    /// Interaction logic for CreateMessageView.xaml
    /// </summary>
    public partial class CreateMessageView : Window
    {
        public CreateMessageView(PersonDTO sender,PersonDTO recipient)
        {
            InitializeComponent();
            DataContext = new CreateMessageViewModel(sender,recipient);
        }
    }
}
