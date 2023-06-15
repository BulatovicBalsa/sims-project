using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Hospital.GUI.CustomControls;

/// <summary>
///     Interaction logic for BindablePasswordBox.xaml
/// </summary>
public partial class BindablePasswordBox : UserControl
{
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register(nameof(Password), typeof(SecureString), typeof(BindablePasswordBox));

    public BindablePasswordBox()
    {
        InitializeComponent();
        TxtPassword.PasswordChanged += OnPasswordChanged;
    }

    public SecureString Password
    {
        get => (SecureString)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = TxtPassword.SecurePassword;
    }
}