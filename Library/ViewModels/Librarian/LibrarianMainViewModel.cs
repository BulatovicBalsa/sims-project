using System.Windows.Input;

namespace Library.ViewModels.Librarian;

public class LibrarianMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    public LibrarianMainViewModel()
    {
        ShowMembersViewCommand = new ViewModelCommand(ExecuteShowMembersViewCommand);
        ShowCopyViewCommand = new ViewModelCommand(ExecuteShowCopyViewCommand);
        ShowBooksViewCommand = new ViewModelCommand(ExecuteShowBooksViewCommand);
        ShowLoansViewCommand = new ViewModelCommand(ExecuteShowLoansViewCommand);
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
    public ICommand ShowCopyViewCommand { get; }
    public ICommand ShowBooksViewCommand { get; }
    public ICommand ShowLoansViewCommand { get; }

    private void ExecuteShowMembersViewCommand(object? obj)
    {
        CurrentChildView = new MemberGridViewModel();
    }

    private void ExecuteShowCopyViewCommand(object? obj)
    {
        CurrentChildView = new CopyGridViewModel();
    }

    private void ExecuteShowBooksViewCommand(object? obj)
    {
        CurrentChildView = new BookGridViewModel();
    }

    private void ExecuteShowLoansViewCommand(object? obj)
    {
        CurrentChildView = new LoanManagementViewModel();
    }
}
