using MTPL.UserPages;
using System.Windows;

namespace MTPL
{
    public partial class MainWindow : Window
    {
        public bool isNav = false;
        public MainWindow()
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
            FrameManager.MainFrame.Navigate(new bid());
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new Insurance());
        }
    }
}
