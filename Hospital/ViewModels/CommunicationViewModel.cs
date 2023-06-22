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
                OnPropertyChanged(nameof(EmailMessages));
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
                LoadMedicalStaff();
            }
        }
        private bool _isAllMessagesSelected;
        public bool IsAllMessagesSelected
        {
            get => _isAllMessagesSelected;
            set
            {
                _isAllMessagesSelected = value;
                if (_isAllMessagesSelected)
                {
                    OnPropertyChanged(nameof(IsAllMessagesSelected));
                    LoadAllEmailMessages();
                }
            }
        }
        private bool _isSentMessagesSelected;
        public bool IsSentMessagesSelected
        {
            get => _isSentMessagesSelected;
            set
            {
                _isSentMessagesSelected = value;
                if (_isSentMessagesSelected)
                {
                    OnPropertyChanged(nameof(IsSentMessagesSelected));
                    LoadSentEmailMessages();
                }
            }
        }
        private bool _isReceivedMessagesSelected;
        public bool IsReceivedMessagesSelected
        {
            get => _isReceivedMessagesSelected;
            set
            {
                _isReceivedMessagesSelected = value;
                if (_isReceivedMessagesSelected)
                {
                    OnPropertyChanged(nameof(IsReceivedMessagesSelected));
                    LoadReceivedEmailMessages();
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
            SearchText = string.Empty;
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
            var allDoctorsAndLibrarians = _emailMessageService.GetMedicalStaffByFilter(_loggedUser.Id,SearchText);
            MedicalStaff = new ObservableCollection<PersonDTO>(allDoctorsAndLibrarians);
        }
        private void SendMessage()
        {
            if (SelectedMedicalStaff != null)
            {
                CreateMessageView createMessageView = new CreateMessageView(_loggedUser, SelectedMedicalStaff);
                createMessageView.ViewModel.MessageSent += CreateMessageView_MessageSent;
                createMessageView.Show();
            }
            else
            {
                MessageBox.Show("Please select a medical staff member before sending a message.", "No Medical Staff Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void CreateMessageView_MessageSent(object sender, EventArgs e)
        {
            if(IsAllMessagesSelected) LoadAllEmailMessages();
            else if(IsSentMessagesSelected) LoadSentEmailMessages();
            else if(IsReceivedMessagesSelected) LoadReceivedEmailMessages();
        }

    }
}
