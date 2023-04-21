using Hospital.Models.Patient;
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
using Microsoft.VisualBasic;
using Hospital.Coordinators;

namespace Hospital.Views
{
    public partial class MedicalRecordPage : Page
    {
        private Patient _patient;
        private readonly DoctorCoordinator _doctorCoordinator;

        public MedicalRecordPage(Patient patient, bool isEditable)
        {
            _patient = patient;
            _doctorCoordinator = new DoctorCoordinator();
            InitializeComponent();
            ConfigPage(patient);
            ConfigEditableGuiElements(isEditable);
        }

        private void ConfigEditableGuiElements(bool isEditable)
        {
            foreach (var obj in LogicalTreeHelper.GetChildren((Grid)FindName("MedicalRecordGrid")))
            {
                if (obj is StackPanel stackPanel)
                {
                    foreach (var stackPanelObj in LogicalTreeHelper.GetChildren(stackPanel))
                    {
                        if (stackPanelObj is Button button)
                        {
                            button.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
                        }
                    }
                }
                if (obj is TextBox textBox)
                {
                    textBox.IsReadOnly = !isEditable;
                }
            }
        }

        private void ConfigPage(Patient patient)
        {
            DataContext = patient.MedicalRecord;
            Title = $"{patient.FirstName} {patient.LastName}";
        }

        private void AddAllergyButton_Click(object sender, RoutedEventArgs e)
        {
            string allergyToAdd = Interaction.InputBox("Insert allergy: ", "Add Allergy", "");
            try
            {
                _patient.MedicalRecord.AddAllergy(allergyToAdd);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            AllergiesListBox.Items.Refresh();
        }

        private void AddMedicalConditionButton_Click(object sender, RoutedEventArgs e)
        {
            string conditionToAdd = Interaction.InputBox("Insert condition: ", "Add condition", "");
            try
            {
                _patient.MedicalRecord.AddConidition(conditionToAdd);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            MedicalHistoryListBox.Items.Refresh();
        }
    }
}
