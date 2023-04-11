using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;


namespace Hospital.ViewModels
{

    public class PatientViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Examination> _examinations;
        private readonly ExaminationRepository _examinationRepository;

        public ObservableCollection<Examination> Examinations
        {
            get { return _examinations;}
            set
            {
                _examinations = value;
                OnPropertyChanged(nameof(Examinations));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PatientViewModel(ExaminationRepository examinationRepository)
        {
            _examinationRepository = examinationRepository;
        }

        public void LoadExaminations(Patient patient)
        {
            var examinations = _examinationRepository.GetAll(patient);
            Examinations = new ObservableCollection<Examination>(examinations);
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
