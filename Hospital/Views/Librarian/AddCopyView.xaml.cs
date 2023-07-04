using System.Windows;
using System.Windows.Input;

namespace Hospital.Views.Librarian
{
    /// <summary>
    /// Interaction logic for AddCopyView.xaml
    /// </summary>
    public partial class AddCopyView : Window
    {
        public AddCopyView()
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
