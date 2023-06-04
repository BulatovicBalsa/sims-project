using Hospital.DTOs;
using Hospital.Models;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class CommunicationViewModel : ViewModelBase
    {
        private EmailMessageService _emailMessageService;
        private PersonDTO _loggedUser;
        private ObservableCollection<EmailMessage> _emailMessages;
        public ObservableCollection<EmailMessage> EmailMessages
        {
            get => _emailMessages;
            set
            {
                _emailMessages = value;
                OnPropertyChanged(nameof(EmailMessage);
            }
        }
        private ObservableCollection<PersonDTO> _medicalStaff;
        public ObservableCollection<PersonDTO> MedicalStaff
        {
            get => _medicalStaff;
            set
            {
                _medicalStaff = value;
                OnPropertyChanged(nameof(MedicalStaff));
            }
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private bool _isAllMessagesSelected;
        public bool IsAllMessagesSelected
        {
            get => _isAllMessagesSelected;
            set
            {
                if (_isAllMessagesSelected != value)
                {
                    _isAllMessagesSelected = value;
                    OnPropertyChanged(nameof(IsAllMessagesSelected));

                    if (_isAllMessagesSelected)
                    {
                        _isSentMessagesSelected = false;
                        _isReceivedMessagesSelected = false;
                        OnPropertyChanged(nameof(IsSentMessagesSelected));
                        OnPropertyChanged(nameof(IsReceivedMessagesSelected));

                        LoadAllEmailMessages();
                    }
                }
            }
        }
        private bool _isSentMessagesSelected;
        public bool IsSentMessagesSelected
        {
            get => _isSentMessagesSelected;
            set
            {
                if (_isSentMessagesSelected != value)
                {
                    _isSentMessagesSelected = value;
                    OnPropertyChanged(nameof(IsSentMessagesSelected));

                    if (_isSentMessagesSelected)
                    {
                        _isAllMessagesSelected = false;
                        _isReceivedMessagesSelected = false;
                        OnPropertyChanged(nameof(IsAllMessagesSelected));
                        OnPropertyChanged(nameof(IsReceivedMessagesSelected));

                        LoadSentEmailMessages();
                    }
                }
            }
        }
        private bool _isReceivedMessagesSelected;
        public bool IsReceivedMessagesSelected
        {
            get => _isReceivedMessagesSelected;
            set
            {
                if (_isReceivedMessagesSelected != value)
                {
                    _isReceivedMessagesSelected = value;
                    OnPropertyChanged(nameof(IsReceivedMessagesSelected));

                    if (_isReceivedMessagesSelected)
                    {
                        _isAllMessagesSelected = false;
                        _isSentMessagesSelected = false;
                        OnPropertyChanged(nameof(IsAllMessagesSelected));
                        OnPropertyChanged(nameof(IsSentMessagesSelected));

                        LoadReceivedEmailMessages();
                    }
                }
            }
        }
        public CommunicationViewModel(PersonDTO user)
        {
            _emailMessageService = new EmailMessageService();
            _loggedUser = user;
            LoadAllEmailMessages();
            LoadMedicalStaff();
        }

        private void LoadAllEmailMessages()
        {
            var emailMessages = _emailMessageService.GetAllEmailMessagesByParticipant(_loggedUser.Id);
            EmailMessages = new ObservableCollection<EmailMessage>(emailMessages);
        }
        private void LoadSentEmailMessages()
        {
            var emailMessages = _emailMessageService.GetSentEmailMessagesByParticipant(_loggedUser.Id);
            EmailMessages = new ObservableCollection<EmailMessage>(emailMessages);
        }
        private void LoadReceivedEmailMessages()
        {
            var emailMessages = _emailMessageService.GetReceivedEmailMessagesByParticipant(_loggedUser.Id);
            EmailMessages = new ObservableCollection<EmailMessage>(emailMessages);
        }
        private void LoadMedicalStaff()
        {
            var allDoctorsAndNurses = PersonRepository.Instance.GetAll()
                .Where(person => person.Role == Role.Doctor || person.Role == Role.Nurse)
                .Where(person => person.Id != _loggedUser.Id)
                .Select(person => new PersonDTO(person.Id, person.FirstName, person.LastName, person.Role));

            MedicalStaff = new ObservableCollection<PersonDTO>(allDoctorsAndNurses);
        }

    }
}
