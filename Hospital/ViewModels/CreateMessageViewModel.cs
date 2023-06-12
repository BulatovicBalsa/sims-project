using Hospital.DTOs;
using Hospital.Services;
using Hospital.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Hospital.Models;

namespace Hospital.ViewModels
{
    public class CreateMessageViewModel : ViewModelBase
    {
        private PersonDTO _sender;
        private PersonDTO _recipient;
        private EmailMessageService _emailMessageService;
        private string _subject;
        private string _message;
        public event EventHandler MessageSent;
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

        public CreateMessageViewModel(PersonDTO sender, PersonDTO recipient)
        {
            _sender = sender;
            _recipient = recipient;
            _emailMessageService = new EmailMessageService();

            SendCommand = new RelayCommand(Send);
        }

        private void Send()
        {
            EmailMessage message = new EmailMessage(_sender, _recipient, _subject, _message);
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
}
