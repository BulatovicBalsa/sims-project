using System;
using System.Windows.Input;

namespace Hospital.GUI.ViewModels;

public class ViewModelCommand : ICommand
{
    private readonly Predicate<object>? _canExecuteAction;
    private readonly Action<object> _executeAction;

    public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = canExecuteAction;
    }

    public ViewModelCommand(Action<object> executeAction)
    {
        _executeAction = executeAction;
        _canExecuteAction = null;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecuteAction?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        _executeAction(parameter);
    }
}