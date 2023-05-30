using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Hospital.Models.Manager;
using Xceed.Wpf.Toolkit;

namespace Hospital.CustomControls;

public partial class RoomForm : UserControl, INotifyPropertyChanged 
{
    public static readonly DependencyProperty RoomProperty  = DependencyProperty.Register(nameof(Room), typeof(Room), typeof(RoomForm));
    public static readonly DependencyProperty TypesProperty  = DependencyProperty.Register(nameof(Types), typeof(ObservableCollection<RoomType>), typeof(RoomForm));
    public RoomForm()
    {
        InitializeComponent();
        Types = new ObservableCollection<RoomType>(Enum.GetValues<RoomType>().ToList());
        Types.Remove(RoomType.Warehouse);
        Validate();
    }

    public ObservableCollection<RoomType> Types { get => (ObservableCollection<RoomType>) GetValue(TypesProperty); set => SetValue(TypesProperty, value); }

    public Room Room { get => (Room) GetValue(RoomProperty); set => SetValue(RoomProperty, value); }
  
    private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        RoomForm control = (RoomForm)obj;

        RoutedPropertyChangedEventArgs<Room> e = new RoutedPropertyChangedEventArgs<Room>(
            (Room)args.OldValue, (Room)args.NewValue, ValueChangedEvent);
        control.OnValueChanged(e);
    }

    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
        "ValueChanged", RoutingStrategy.Bubble,
        typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(RoomForm));

    private bool _hasErrors;

    public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
    {
        add { AddHandler(ValueChangedEvent, value); }
        remove { RemoveHandler(ValueChangedEvent, value); }
    }

    protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<Room> args)
    {
        RaiseEvent(args);
    }


    private bool IsValid(DependencyObject obj)
    {
        // The dependency object is valid if it has no errors and all
        // of its children (that are dependency objects) are error-free.
        return !Validation.GetHasError(obj) &&
        LogicalTreeHelper.GetChildren(obj)
        .OfType<DependencyObject>()
        .All(IsValid);
    }

    public bool HasErrors
    {
        get => _hasErrors;
        private set
        {
            if (value == _hasErrors) return;
            _hasErrors = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void RoomNameChanged(object sender, TextChangedEventArgs e)
    {
        Validate();
    }

    private void Validate()
    {
        HasErrors = !IsValid(this);
    }
}