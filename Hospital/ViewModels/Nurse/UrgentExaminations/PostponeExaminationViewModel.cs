using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hospital.Models.Examination;

namespace Hospital.ViewModels.Nurse.UrgentExaminations
{
    public class PostponeExaminationViewModel : ViewModelBase
    {
        private Examination _selectedExamination;

        public PostponeExaminationViewModel()
        {
            // dummy constructor
        }

        public PostponeExaminationViewModel(List<Examination> examinations)
        {
            Examinations = examinations;
            _selectedExamination = null;
        }

        public List<Examination> Examinations { get; }
        public Examination SelectedExamination
        {
            get => _selectedExamination;
            set
            {
                _selectedExamination = value;
                OnPropertyChanged(nameof(SelectedExamination));
            }
        }

        public ICommand PostponeExaminationCommand { get; }
        public ICommand CloseDialogCommand { get; }
    }
}
