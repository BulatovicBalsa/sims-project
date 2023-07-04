using System.Threading;
using System.Windows.Input;
using Hospital.DTOs;
using Hospital.Services;
using Hospital.Views;

namespace Hospital.ViewModels.Librarian;

public class LibrarianMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    private LibrarianService _librarianService = new();
    public LibrarianMainViewModel()
    {
        ShowMembersViewCommand = new ViewModelCommand(ExecuteShowMembersViewCommand);

        ExecuteShowMembersViewCommand(null);
    }

    public ViewModelBase CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged(nameof(CurrentChildView));
        }
    }
    public ICommand ShowMembersViewCommand { get; }

    private void ExecuteShowMembersViewCommand(object? obj)
    {
        CurrentChildView = new MemberGridViewModel();
    }

}
