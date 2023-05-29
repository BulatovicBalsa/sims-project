using System;
using System.Windows;
using System.Windows.Controls;
using Hospital.Models.Manager;
using Xceed.Wpf.Toolkit;

namespace Hospital.CustomControls;

public partial class RoomForm : UserControl
{
    public static readonly DependencyProperty RoomProperty = DependencyProperty.Register(nameof(Room), typeof(Room), typeof(RoomForm));
    public RoomForm()
    {
        InitializeComponent();
    }

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

    public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
    {
        add { AddHandler(ValueChangedEvent, value); }
        remove { RemoveHandler(ValueChangedEvent, value); }
    }

    protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<Room> args)
    {
        RaiseEvent(args);
    }
}