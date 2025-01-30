using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace MTPL.Authentication
{
    public partial class Reg : Window
    {
        private static readonly Regex LoginRegex = new Regex(@"^[a-zA-Z0-9]{6,}$");
        private static readonly Regex PasswordRegex = new Regex(@"^(?=.*\d)[a-zA-Z0-9]{6,}$");

        public bool isNav = false;
        private readonly MTPLEntities db;
        public Reg()
        {
            InitializeComponent();
            db = new MTPLEntities();
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
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

                if (db.users.Any(u => u.login == Login))
                {
                    MessageBox.Show("Аккаунт с таким логином уже существует\n" +
                        "Попробуйте другой",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var users = new user
                {
                    login = Login,
                    password = password,
                    role_id = 1
                };

                db.users.Add(users);
                db.SaveChanges();
                MessageBox.Show("Вы зарегистрированы в систему!",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                isNav = true;
                Login login = new Login();
                login.Show();
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

        private void NavToLogin_Click(object sender, RoutedEventArgs e)
        {
            isNav = true;
            Login login = new Login();
            login.Show();
            this.Close();
        }

    }
}