using System.Windows;
using System.Windows.Input;

namespace Hospital.Views
{
    public partial class ModifyExaminationDialog : Window
    {
        public ModifyExaminationDialog()
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
