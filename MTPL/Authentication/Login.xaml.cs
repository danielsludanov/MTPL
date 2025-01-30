using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace MTPL.Authentication
{
    public partial class Login : Window
    {
        private static readonly Regex LoginRegex = new Regex(@"^[a-zA-Z0-9]{6,}$");
        private static readonly Regex PasswordRegex = new Regex(@"^(?=.*\d)[a-zA-Z0-9]{6,}$");

        private readonly MTPLEntities db;
        public bool isNav = false;

        public Login()
        {
            InitializeComponent();
            db = new MTPLEntities();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string Login = UserLogin.Text;
            string password = UserPassword.Password;

            try
            {
                if (!LoginRegex.IsMatch(Login))
                {
                    MessageBox.Show("Вы неправильно ввели логин",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Логин содержит как и английские символы, так и цифры\n" +
                        "Минимальное количество символов в логине: 6",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (!PasswordRegex.IsMatch(password))
                {
                    MessageBox.Show("Вы неправильно ввели пароль",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Пароль содержит как и английские символы, так и цифры\n" +
                        "Минимальное количество символов в пароле: 6",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var userToCheck = db.users.AsNoTracking().FirstOrDefault(x => x.login == Login && x.password == password);

                if (userToCheck == null)
                {
                    MessageBox.Show("Пользователя с таким логином не существует в системе",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Вы авторизовались!");
                
                isNav = true;
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void NavToReg_Click(object sender, RoutedEventArgs e)
        {
            isNav = true;
            Reg reg = new Reg();
            reg.Show();
            this.Close();
            
        }
    }
}