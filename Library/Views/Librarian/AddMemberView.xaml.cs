using System.Windows;
using System.Windows.Input;

namespace Library.Views.Librarian
{
    /// <summary>
    /// Interaction logic for AddMemberView.xaml
    /// </summary>
    public partial class AddMemberView : Window
    {
        public AddMemberView()
        {
            InitializeComponent();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ControlBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
