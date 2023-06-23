using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.DTOs;
using Hospital.Exceptions;
using Hospital.Models.Books;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Requests;
using Hospital.Services;
using Hospital.Services.Books;
using Hospital.Services.Requests;
using Hospital.ViewModels.Books;
using Hospital.Views;

namespace Hospital.ViewModels;

public class DoctorViewModel : ViewModelBase
{
    private const string Placeholder = "Search...";
    private readonly BookService _bookService = new();
    private readonly LoanService _loanService = new();
    private readonly PatientService _patientService = new();
    private readonly DoctorTimeOffRequestService _requestService = new();

    private ObservableCollection<Book> _books;

    private Doctor _doctor;
    private ObservableCollection<Loan> _loans;

    private string? _searchBoxText;

    private DateTime _selectedDate;

    private object? _selectedBook;
    private ObservableCollection<DoctorTimeOffRequest> _timeOffRequests;

    public DoctorViewModel(Doctor doctor)
    {
        _doctor = doctor;
        _selectedDate = DateTime.Now;
        Books = new ObservableCollection<Book>(_bookService.GetDistinct());
        _books = Books;
        TimeOffRequests =
            new ObservableCollection<DoctorTimeOffRequest>(_requestService.GetNonExpiredDoctorTimeOffRequests(doctor));
        _timeOffRequests = TimeOffRequests;
        Loans =
            new ObservableCollection<Loan>(_loanService.GetAll(doctor));
        _loans = Loans;
        SearchBoxText = Placeholder;

        ViewAdvancedBookDetailsCommand = new RelayCommand<string>(ViewAdvancedBookDetails);
        AddExaminationCommand = new RelayCommand(AddExamination);
        UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        DeleteExaminationCommand = new RelayCommand(DeleteExamination);
        PerformExaminationCommand = new RelayCommand(PerformExamination);
        DefaultExaminationViewCommand = new RelayCommand(DefaultExaminationView);
        AddTimeOffRequestCommand = new RelayCommand(AddTimeOffRequest);
        VisitHospitalizedPatientsCommand = new RelayCommand(VisitHospitalizedPatients);
        SendMessageCommand = new RelayCommand(SendMessage);
        AddTimeOffRequestCommand = new RelayCommand(AddTimeOffRequest);
        VisitHospitalizedPatientsCommand = new RelayCommand(VisitHospitalizedPatients);
    }

    public ObservableCollection<Loan> Loans
    {
        get => _loans;
        set
        {
            _loans = value;
            OnPropertyChanged(nameof(Loans));
        }
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            _books = value;
            OnPropertyChanged(nameof(Books));
        }
    }

    public ObservableCollection<DoctorTimeOffRequest> TimeOffRequests
    {
        get => _timeOffRequests;
        set
        {
            _timeOffRequests = value;
            OnPropertyChanged(nameof(TimeOffRequests));
        }
    }

    public Doctor Doctor
    {
        get => _doctor;
        set
        {
            _doctor = value;
            OnPropertyChanged(nameof(Doctor));
        }
    }

    public string? SearchBoxText
    {
        get => _searchBoxText;
        set
        {
            _searchBoxText = value;
            OnPropertyChanged(SearchBoxText);
        }
    }

    public object? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            OnPropertyChanged(nameof(SelectedBook));
        }
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
            Loans.Clear();
            //_loanService.GetExaminationsForDate(_doctor, SelectedDate).ToList().ForEach(Loans.Add);
        }
    }

    public object SelectedExamination { get; set; }

    public string DoctorName => $"Doctor {Doctor.FirstName} {Doctor.LastName}";

    public ICommand ViewAdvancedBookDetailsCommand { get; set; }
    public ICommand AddExaminationCommand { get; set; }
    public ICommand PerformExaminationCommand { get; set; }
    public ICommand UpdateExaminationCommand { get; set; }
    public ICommand DeleteExaminationCommand { get; set; }
    public ICommand DefaultExaminationViewCommand { get; set; }
    public ICommand SendMessageCommand { get; set; }
    public ICommand AddTimeOffRequestCommand { get; set; }
    public ICommand VisitHospitalizedPatientsCommand { get; set; }

    private void ViewAdvancedBookDetails(string bookId)
    {
        var book = _bookService.GetBookById(bookId);
        if (book == null)
        {
            MessageBox.Show("Select a book in order to see more details", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var dialog = new AdvancedBookDetailsDialog()
        {
            DataContext = new AdvancedBookDetailsViewModel(book)
        };
        dialog.ShowDialog();
    }

    private void DefaultExaminationView()
    {
        Loans.Clear();
        _loanService.GetAll(_doctor).ForEach(Loans.Add);
    }

    private void VisitHospitalizedPatients()
    {
        var dialog = new VisitHospitalizedPatientsDialog(_doctor);
        dialog.ShowDialog();
    }

    private void AddTimeOffRequest()
    {
        var dialog = new AddTimeOffRequestDialog(_doctor);
        dialog.ShowDialog();
        TimeOffRequests =
            new ObservableCollection<DoctorTimeOffRequest>(_requestService.GetNonExpiredDoctorTimeOffRequests(_doctor));
    }

    private void ViewMedicalRecord(string patientId)
    {
        var patient = _patientService.GetPatientById(patientId);
        if (patient == null)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        var dialog = new MedicalRecordDialog(patient, false);
        dialog.ShowDialog();
    }

    private void AddExamination()
    {
        var dialog = new ModifyExaminationDialog(Doctor, Loans)
        {
            WindowStyle = WindowStyle.ToolWindow
        };

        var result = dialog.ShowDialog();
        if (result == true) MessageBox.Show("Succeed");
    }

    private void UpdateExamination()
    {
        var examinationToChange = SelectedExamination as Examination;
        if (examinationToChange == null)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        var dialog = new ModifyExaminationDialog(Doctor, Loans, examinationToChange)
        {
            WindowStyle = WindowStyle.ToolWindow
        };

        var result = dialog.ShowDialog();

        if (result == true) MessageBox.Show("Succeed");
    }

    private void DeleteExamination()
    {
        var examination = SelectedExamination as Examination;
        if (examination == null)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        try
        {
            _loanService.DeleteExamination(examination, false);
        }
        catch (DoctorNotBusyException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        catch (BookNotLoanedException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        Loans.Remove(examination);
        MessageBox.Show("Succeed");
    }

    private void PerformExamination()
    {
        var examinationToPerform = SelectedExamination as Examination;
        if (examinationToPerform == null)
        {
            MessageBox.Show("Please select examination in order to perform it");
            return;
        }

        var patientOnExamination = _patientService.GetPatient(examinationToPerform);

        if (!examinationToPerform.IsPerformable())
        {
            MessageBox.Show("Chosen examination can't be performed right now");
            return;
        }

        if (examinationToPerform.Room is null)
        {
            MessageBox.Show("Chosen examination doesn't have room. Please add room");
            return;
        }

        var dialog = new PerformExaminationDialog(examinationToPerform, patientOnExamination);
        dialog.ShowDialog();
    }

    private void SendMessage()
    {
        var loggedUser = new PersonDTO(_doctor.Id, _doctor.FirstName, _doctor.LastName, Role.Doctor);
        var communicationView = new CommunicationView(loggedUser);
        communicationView.ShowDialog();
    }
}