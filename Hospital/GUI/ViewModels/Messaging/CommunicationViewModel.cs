using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.DTOs;
using Hospital.GUI.Views.Messaging;
using Hospital.Messaging.Models;
using Hospital.Messaging.Services;

namespace Hospital.GUI.ViewModels.Messaging;

public class CommunicationViewModel : ViewModelBase
{
    private ObservableCollection<EmailMessage> _emailMessages;
    private readonly EmailMessageService _emailMessageService;
    private bool _isAllMessagesSelected;
    private bool _isReceivedMessagesSelected;
    private bool _isSentMessagesSelected;
    private readonly PersonDTO _loggedUser;
    private ObservableCollection<PersonDTO> _medicalStaff;
    private string _searchText;
    private PersonDTO _selectedMedicalStaff;

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

    public ObservableCollection<EmailMessage> EmailMessages
    {
        get => _emailMessages;
        set
        {
            _emailMessages = value;
            OnPropertyChanged(nameof(EmailMessages));
        }
    }

    public ObservableCollection<PersonDTO> MedicalStaff
    {
        get => _medicalStaff;
        set
        {
            _medicalStaff = value;
            OnPropertyChanged(nameof(MedicalStaff));
        }
    }

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
        var allDoctorsAndNurses = _emailMessageService.GetMedicalStaffByFilter(_loggedUser.Id, SearchText);
        MedicalStaff = new ObservableCollection<PersonDTO>(allDoctorsAndNurses);
    }

    private void SendMessage()
    {
        if (SelectedMedicalStaff != null)
        {
            var createMessageView = new CreateMessageView(_loggedUser, SelectedMedicalStaff);
            createMessageView.ViewModel.MessageSent += CreateMessageView_MessageSent;
            createMessageView.Show();
        }
        else
        {
            MessageBox.Show("Please select a medical staff member before sending a message.",
                "No Medical Staff Selected", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void CreateMessageView_MessageSent(object sender, EventArgs e)
    {
        if (IsAllMessagesSelected) LoadAllEmailMessages();
        else if (IsSentMessagesSelected) LoadSentEmailMessages();
        else if (IsReceivedMessagesSelected) LoadReceivedEmailMessages();
    }
}