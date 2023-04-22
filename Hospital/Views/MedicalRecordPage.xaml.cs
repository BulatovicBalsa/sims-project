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
using System.Text.RegularExpressions;

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

            LogicalTreeHelper.GetChildren(MedicalRecordGrid)
                .OfType<StackPanel>()
                .ToList()
                .ForEach(sp => updateStackPanelButtonsVisibility(sp, isEditable));

            LogicalTreeHelper.GetChildren(MedicalRecordGrid)
                .OfType<TextBox>()
                .ToList()
                .ForEach(tb => tb.IsReadOnly = !isEditable);

            LogicalTreeHelper.GetChildren(MedicalRecordGrid)
                .OfType<Button>()
                .ToList()
                .ForEach(btn => btn.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed);
        }

        private void updateStackPanelButtonsVisibility(StackPanel stackPanel, bool isEditable)
        {
            var buttons = stackPanel.Children.OfType<Button>();

            foreach (var button in buttons)
            {
                button.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
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
                _patient.MedicalRecord.AddMedicalConidition(conditionToAdd);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            MedicalHistoryListBox.Items.Refresh();
        }

        private void UpdateAllergyButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedAllergy = (string)AllergiesListBox.SelectedItem;
            if (selectedAllergy == null)
            {
                MessageBox.Show("You must select condition in order to update it");
                return;
            }
            string updatedAllergy = Interaction.InputBox($"Update '{selectedAllergy}' name: ", "Update allergy", "");
            try
            {
                _patient.MedicalRecord.UpdateAllergy(selectedAllergy, updatedAllergy);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            AllergiesListBox.Items.Refresh();
        }

        private void UpdateMedicalConditionButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedCondition = (string)MedicalHistoryListBox.SelectedItem;
            if (selectedCondition == null)
            {
                MessageBox.Show("You must select condition in order to update it");
                return;
            }
            string updatedCondition = Interaction.InputBox($"Update '{selectedCondition}' name: ", "Update condition", "");
            try
            {
                _patient.MedicalRecord.UpdateMedicalCondition(selectedCondition, updatedCondition);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            MedicalHistoryListBox.Items.Refresh();
        }

        private void DeleteMedicalConditionButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedCondition = (string)MedicalHistoryListBox.SelectedItem;
            if (selectedCondition == null)
            {
                MessageBox.Show("You must select condition in order to delete it");
                return;
            }
            try
            {
                _patient.MedicalRecord.DeleteMedicalCondition(selectedCondition);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _doctorCoordinator.UpdatePatient(_patient);
            MedicalHistoryListBox.Items.Refresh();
        }

        private void DeleteAllergyButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedAllergy = (string)AllergiesListBox.SelectedItem;
            if (selectedAllergy == null)
            {
                MessageBox.Show("You must select allergy in order to delete it");
                return;
            }
            try
            {
                _patient.MedicalRecord.DeleteAllergy(selectedAllergy);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _doctorCoordinator.UpdatePatient(_patient);
            AllergiesListBox.Items.Refresh();
        }

        private void ChangeWeightButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePhysicalCharacteristic(false);
        }

        private void ChangeHeightButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePhysicalCharacteristic(true);
        }

        private void ChangePhysicalCharacteristic(bool isHeight)
        {
            var textBox = isHeight ? HeightTextBox : WeightTextBox;
            int newSize = Int32.Parse(textBox.Text);
            try
            {
                if (isHeight)
                    _patient.MedicalRecord.ChangeHeight(newSize);
                else
                    _patient.MedicalRecord.ChangeWeight(newSize);    
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _doctorCoordinator.UpdatePatient(_patient);
            MessageBox.Show("Succeed");
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }
    }
}
