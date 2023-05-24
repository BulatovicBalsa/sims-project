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
using Hospital.Models.Patient;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class AddPrescriptionDialog : Window
    {
        public AddPrescriptionDialog(Patient patientOnExamination, HospitalTreatmentReferral? referralToModify)
        {
            DataContext = new AddPrescriptionViewModel(patientOnExamination, referralToModify);
            InitializeComponent();
        }
    }
}