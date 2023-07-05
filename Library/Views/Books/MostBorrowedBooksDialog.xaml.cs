using System.Windows;
using System.Windows.Input;

namespace Library.Views.Books
{
    /// <summary>
    /// Interaction logic for MostBorrowedBooksDialog.xaml
    /// </summary>
    public partial class MostBorrowedBooksDialog : Window
    {
        public MostBorrowedBooksDialog()
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
