using MTPL.UserPages;
using System.Windows;

namespace MTPL
{

    public partial class EmployeeWindow : Window
    {
        public bool isNav = false;
        public EmployeeWindow()
        {
            InitializeComponent();
            FrameManager.MainFrame = MainFrame;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isNav)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотите выйти из приложения?",
                "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void bidClick_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new Policies());
        }
    }
}
