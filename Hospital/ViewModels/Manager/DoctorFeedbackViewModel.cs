using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.DTOs;
using Hospital.Models.Doctor;
using Hospital.Models.Feedback;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Feedback;

namespace Hospital.ViewModels.Manager
{
    public class DoctorFeedbackViewModel: ViewModelBase
    {
        private ObservableCollection<DoctorFeedback> _allFeedback;
        private ObservableCollection<DoctorFeedback> _selectedDoctorFeedback;
        private string _selectedDoctorId;
        private ObservableCollection<Doctor> _doctors;

        public ObservableCollection<DoctorFeedback> AllFeedback
        {
            get => _allFeedback;
            set
            {
                if (Equals(value, _allFeedback)) return;
                _allFeedback = value;
                OnPropertyChanged(nameof(AllFeedback));
            }
        }

        public ObservableCollection<DoctorFeedback> SelectedDoctorFeedback
        {
            get => _selectedDoctorFeedback;
            set
            {
                if (Equals(value, _selectedDoctorFeedback)) return;
                _selectedDoctorFeedback = value;
                OnPropertyChanged(nameof(SelectedDoctorFeedback));
            }
        }

        public string SelectedDoctorId
        {
            get => _selectedDoctorId;
            set
            {
                if (value == _selectedDoctorId) return;
                _selectedDoctorId = value;
                SelectedDoctorFeedback =
                    new ObservableCollection<DoctorFeedback>(
                        DoctorFeedbackRepository.Instance.GetByDoctorId(_selectedDoctorId));
                OnPropertyChanged(nameof(SelectedDoctorId));
            }
        }

        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set
            {
                if (Equals(value, _doctors)) return;
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        public DoctorFeedbackViewModel()
        {
            AllFeedback = new ObservableCollection<DoctorFeedback>(DoctorFeedbackRepository.Instance.GetAll());
            Doctors = new ObservableCollection<Doctor>(DoctorRepository.Instance.GetAll());
            SelectedDoctorId = "";
            SelectedDoctorFeedback = new ObservableCollection<DoctorFeedback>();
        }
    }
}
