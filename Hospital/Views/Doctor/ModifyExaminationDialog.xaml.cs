﻿using Hospital.Services;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
using Hospital.Models.Books;

namespace Hospital.Views
{
    public partial class ModifyExaminationDialog : Window
    {
        public ModifyExaminationDialog()
        {
            InitializeComponent();
        }
    }
}
