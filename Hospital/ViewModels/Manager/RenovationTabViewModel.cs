using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Views.Manager;

namespace Hospital.ViewModels.Manager;

public class RenovationTabViewModel : ViewModelBase
{
    private readonly Timer _refreshTimer;
    private ObservableCollection<Renovation> _renovations;

    public RenovationTabViewModel()
    {
        _renovations = new ObservableCollection<Renovation>(RenovationRepository.Instance.GetAll());
        OpenAddSimpleRenovationFormCommand = new RelayCommand(OpenAddSimpleRenovationForm);
        _refreshTimer = new Timer();
        StartAutoRefresh();
    }

    public ObservableCollection<Renovation> Renovations
    {
        get => _renovations;
        set
        {
            if (Equals(value, _renovations)) return;
            _renovations = value;
            OnPropertyChanged(nameof(Renovations));
        }
    }

    public ICommand OpenAddSimpleRenovationFormCommand { get; set; }

    private void StartAutoRefresh()
    {
        _refreshTimer.Interval = 1000;
        _refreshTimer.AutoReset = true;
        _refreshTimer.Enabled = true;
        _refreshTimer.Elapsed += (sender, args) => Refresh();
    }

    private void Refresh()
    {
        Renovations = new ObservableCollection<Renovation>(RenovationRepository.Instance.GetAll());
    }

    public void OpenAddSimpleRenovationForm()
    {
        var addRenovationForm = new AddRenovation();
        addRenovationForm.Closed += (sender, args) => Refresh();
        addRenovationForm.Show();
    }
}