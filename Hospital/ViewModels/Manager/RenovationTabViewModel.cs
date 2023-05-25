using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Views.Manager;

namespace Hospital.ViewModels.Manager;

public class RenovationTabViewModel : ViewModelBase
{
    private BindingList<Renovation> _renovations;

    public RenovationTabViewModel()
    {
        _renovations = new BindingList<Renovation>(RenovationRepository.Instance.GetAll());
        OpenAddSimpleRenovationFormCommand = new RelayCommand(OpenAddSimpleRenovationForm);
    }

    public BindingList<Renovation> Renovations
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

    private void Refresh()
    {
        Renovations = new BindingList<Renovation>(RenovationRepository.Instance.GetAll());
    }

    public void OpenAddSimpleRenovationForm()
    {
        var addRenovationForm = new AddRenovation();
        addRenovationForm.Closed += ((sender, args) => Refresh());
        addRenovationForm.Show();
    } 
}