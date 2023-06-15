using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.Accounts.DTOs;
using Hospital.Core.Messaging.Models;
using Hospital.Core.Messaging.Services;
using Hospital.GUI.Views.Messaging;

namespace Hospital.GUI.ViewModels.Messaging;

public class CreateMessageViewModel : ViewModelBase
{
    private readonly EmailMessageService _emailMessageService;
    private string _message;
    private readonly PersonDTO _recipient;
    private readonly PersonDTO _sender;
    private string _subject;

    public CreateMessageViewModel(PersonDTO sender, PersonDTO recipient)
    {
        _sender = sender;
        _recipient = recipient;
        _emailMessageService = new EmailMessageService();

        SendCommand = new RelayCommand(Send);
    }

    public string SenderDisplayName => _sender?.ToString();
    public string RecipientDisplayName => _recipient?.ToString();

    public string Subject
    {
        get => _subject;
        set
        {
            _subject = value;
            OnPropertyChanged(nameof(Subject));
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public ICommand SendCommand { get; }
    public event EventHandler MessageSent;

    private void Send()
    {
        var message = new EmailMessage(_sender, _recipient, _subject, _message);
        _emailMessageService.SendMessage(message);

        MessageSent?.Invoke(this, EventArgs.Empty);

        CloseWindow();
    }

    private void CloseWindow()
    {
        Window createMessageView = Application.Current.Windows.OfType<CreateMessageView>().FirstOrDefault();
        createMessageView?.Close();
    }
}