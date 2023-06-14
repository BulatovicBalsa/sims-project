using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Requests;
using Hospital.Repositories.Requests;
using Hospital.Services.Requests;

namespace Hospital.ViewModels.Manager;

public class DoctorTimeOffRequestViewModel : ViewModelBase
{
    private readonly RelayCommand _acceptCommand;
    private readonly RelayCommand _rejectCommand;
    private readonly DoctorTimeOffRequestService _timeOffRequestService;
    private DoctorTimeOffRequest? _selectedRequest;
    private ObservableCollection<DoctorTimeOffRequest> _timeOffRequests;

    public DoctorTimeOffRequestViewModel()
    {
        TimeOffRequests =
            new ObservableCollection<DoctorTimeOffRequest>(DoctorTimeOffRequestRepository.Instance.GetAll());
        _timeOffRequestService = new DoctorTimeOffRequestService();
        _acceptCommand = new RelayCommand(AcceptSelected, CanAcceptSelected);
        _rejectCommand = new RelayCommand(RejectSelected, CanRejectSelected);
    }

    public ObservableCollection<DoctorTimeOffRequest> TimeOffRequests
    {
        get => _timeOffRequests;
        set
        {
            if (Equals(value, _timeOffRequests)) return;
            _timeOffRequests = value;
            OnPropertyChanged(nameof(TimeOffRequests));
        }
    }

    public DoctorTimeOffRequest? SelectedRequest
    {
        get => _selectedRequest;
        set
        {
            if (Equals(value, _selectedRequest)) return;
            _selectedRequest = value;
            RaiseCanExecuteChangedForAllCommands();
            OnPropertyChanged(nameof(SelectedRequest));
        }
    }

    public ICommand AcceptCommand => _acceptCommand;

    public ICommand RejectCommand => _rejectCommand;

    private void RaiseCanExecuteChangedForAllCommands()
    {
        _acceptCommand.RaiseCanExecuteChanged();
        _rejectCommand.RaiseCanExecuteChanged();
    }

    private void RefreshTimeOffRequests()
    {
        TimeOffRequests =
            new ObservableCollection<DoctorTimeOffRequest>(DoctorTimeOffRequestRepository.Instance.GetAll());
        SelectedRequest = null;
    }

    private void AcceptSelected()
    {
        if (SelectedRequest != null)
            _timeOffRequestService.Accept(SelectedRequest);
        RaiseCanExecuteChangedForAllCommands();
        RefreshTimeOffRequests();
    }

    private void RejectSelected()
    {
        if (SelectedRequest != null)
            _timeOffRequestService.Reject(SelectedRequest);
        RaiseCanExecuteChangedForAllCommands();
        RefreshTimeOffRequests();
    }

    private bool CanAcceptSelected()
    {
        return SelectedRequest is { IsApproved: false };
    }

    private bool CanRejectSelected()
    {
        return SelectedRequest != null;
    }
}