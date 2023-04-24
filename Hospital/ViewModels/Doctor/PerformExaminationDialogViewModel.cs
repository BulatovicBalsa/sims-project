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

namespace Hospital.ViewModels
{
    public class PerformExaminationDialogViewModel : ViewModelBase
    {
        private Examination _examinationToPerform { get; set; }
        private readonly DoctorCoordinator _doctorCoordinator = new DoctorCoordinator();

        private string _anamnesis;
        public string Anamnesis
        {
            get { return _anamnesis; }
            set { _anamnesis = value; OnPropertyChanged(nameof(Anamnesis)); }
        }

        public ICommand UpdateExaminationCommand { get; set; }
        public PerformExaminationDialogViewModel()
        {
            UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        }

        private void UpdateExamination()
        {
            _examinationToPerform.Anamnesis = Anamnesis;
            _doctorCoordinator.UpdateExamination(_examinationToPerform);
            MessageBox.Show("Succeed");
        }
    }
}
