using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels.Doctor
{
    public class PerformExaminationDialogViewModel : ViewModelBase
    {
        private Examination _examinationToPerform { get; set; }
        private readonly DoctorCoordinator _doctorCoordinator = new DoctorCoordinator();

        public string Anamnesis
        {
            get { return _examinationToPerform.Anamnesis; }
            set { _examinationToPerform.Anamnesis = value; OnPropertyChanged(nameof(Anamnesis)); }
        }

        public ICommand UpdateAnamnesisCommand { get; set; }
        public PerformExaminationDialogViewModel()
        {
            UpdateAnamnesisCommand = new RelayCommand(UpdateExamination);
        }

        private void UpdateExamination()
        {
            _doctorCoordinator.UpdateExamination(_examinationToPerform);
            MessageBox.Show("Succeed");
        }
    }
}
