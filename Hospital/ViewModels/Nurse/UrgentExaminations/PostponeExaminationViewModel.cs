using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Repositories.Examinaton;

namespace Hospital.ViewModels.Nurse.UrgentExaminations
{
    public class PostponeExaminationViewModel : ViewModelBase
    {
        private readonly ExaminationRepository _examinationRepository;
        private Examination? _selectedExamination;
        private readonly Dictionary<Doctor, DateTime> _doctorEarliestFree;

        public PostponeExaminationViewModel()
        {
            // dummy constructor
        }

        public PostponeExaminationViewModel(List<Examination> examinations, List<KeyValuePair<Doctor, DateTime>> doctorEarliestFree)
        {
            Examinations = examinations;
            _selectedExamination = null;
            _doctorEarliestFree = doctorEarliestFree.ToDictionary(pair => pair.Key, pair => pair.Value);
            _examinationRepository = new ExaminationRepository();

            PostponeExaminationCommand =
                new ViewModelCommand(ExecutePostponeExaminationCommand, CanExecutePostponeExaminationCommand);
            CloseDialogCommand = new ViewModelCommand(ExecuteCloseDialogCommand);
        }

        public List<Examination> Examinations { get; }
        public Examination? SelectedExamination
        {
            get => _selectedExamination;
            set
            {
                _selectedExamination = value;
                OnPropertyChanged(nameof(SelectedExamination));
            }
        }

        public event Action<bool, DateTime?, Doctor?>? DialogClosed;

        public ICommand PostponeExaminationCommand { get; }
        public ICommand CloseDialogCommand { get; }

        private void ExecutePostponeExaminationCommand(object obj)
        {
            var previousStart = SelectedExamination.Start;
            SelectedExamination.Start = _doctorEarliestFree[SelectedExamination.Doctor];
            _examinationRepository.Update(SelectedExamination, false);

            CloseDialog(false, previousStart, SelectedExamination.Doctor);
        }

        private bool CanExecutePostponeExaminationCommand(object obj)
        {
            return SelectedExamination != null;
        }

        private void ExecuteCloseDialogCommand(object obj)
        {
            CloseDialog(true, null, null);
        }

        private void CloseDialog(bool cancelled, DateTime? newTimeslot, Doctor? freeDoctor)
        {
            Application.Current.Windows[1]?.Close();
            DialogClosed?.Invoke(cancelled, newTimeslot, freeDoctor);
        }
    }
}
