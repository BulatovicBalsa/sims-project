using GalaSoft.MvvmLight.Command;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Services;
using Hospital.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels
{
    public class CommunicationViewModel : ViewModelBase
    {
        private EmailMessageService _emailMessageService;
        private PersonDTO _loggedUser;
        private PersonDTO _selectedMedicalStaff;
        private ObservableCollection<EmailMessage> _emailMessages;
        public ObservableCollection<EmailMessage> EmailMessages
        {
            get => _emailMessages;
            set
            {
                _emailMessages = value;
                OnPropertyChanged(nameof(EmailMessage));
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
        public PersonDTO SelectedMedicalStaff
        {
            get => _selectedMedicalStaff;
            set
            {
                _selectedMedicalStaff = value;
                OnPropertyChanged(nameof(SelectedMedicalStaff));
            }
        }
        public ICommand SendMessageCommand { get; }
        public CommunicationViewModel(PersonDTO user)
        {
            _emailMessageService = new EmailMessageService();
            _loggedUser = user;
            _isAllMessagesSelected = true;
            SendMessageCommand = new RelayCommand(SendMessage);
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
            var allDoctorsAndNurses = _emailMessageService.GetFilteredMedicalStaff(_loggedUser.Id);
            MedicalStaff = new ObservableCollection<PersonDTO>(allDoctorsAndNurses);
        }
        private void SendMessage()
        {
            if (SelectedMedicalStaff != null)
            {
                CreateMessageView createMessageView = new CreateMessageView(_loggedUser, SelectedMedicalStaff);
                createMessageView.Show();
            }
            else
            {
                MessageBox.Show("Please select a medical staff member before sending a message.", "No Medical Staff Selected", MessageBoxButton.OK, MessageBoxImage.Information);


            }
        }

    }
}
