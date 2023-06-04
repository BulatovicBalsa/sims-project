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
        private string _from;
        private string _to;
        private string _subject;
        private string _message;

        public string From
        {
            get => _from;
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
            }
        }

        public string To
        {
            get => _to;
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
            }
        }

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

            From = _sender.FirstName+ " " + _sender.LastName;
            To = _recipient.FirstName + " " + _recipient.LastName;

            SendCommand = new RelayCommand(Send);
        }

        private void Send()
        {
            EmailMessage message = new EmailMessage(_sender, _recipient, _subject, _message);
            _emailMessageService.SendMessage(message);

            CloseWindow();
        }

        private void CloseWindow()
        {
            CreateMessageView? createMessageView = Application.Current.MainWindow as CreateMessageView;
            createMessageView?.Close();
        }
    }
}
