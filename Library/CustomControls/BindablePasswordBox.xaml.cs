using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Library.CustomControls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = 
            DependencyProperty.Register(nameof(Password), typeof(SecureString), typeof(BindablePasswordBox));

        public SecureString Password
        {
            get => (SecureString)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
            TxtPassword.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = TxtPassword.SecurePassword;
        }
    }
}
