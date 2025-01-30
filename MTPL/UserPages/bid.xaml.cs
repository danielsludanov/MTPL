using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MTPL.UserPages
{
    public partial class bid : Page
    {
        
        private static readonly Regex FIORegex = new Regex(@"^[А-Яа-яA-Za-z\s\-]+$");
        private static readonly Regex SerialDriverNumberRegex = new Regex(@"^[A-Za-z]{2}$");
        private static readonly Regex NumberDriverRegex = new Regex(@"^\d{6}$");
        private static readonly Regex SerialPassportRegex = new Regex(@"^\d{2}$");
        private static readonly Regex NumberPassportRegex = new Regex(@"^\d{10}$");

        private readonly MTPLEntities db;

        public bid()
        {
            InitializeComponent();
            db = new MTPLEntities();
            LoadRegions();
        }

        private void LoadRegions()
        {
            var regions = db.regions.ToList();
            RegionBox.ItemsSource = regions;
            RegionBox.DisplayMemberPath = "region_name";
            RegionBox.SelectedValuePath = "region_id";
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(FullName.Text) ||
                    DateBirth.SelectedDate == null ||
                    RegionBox.SelectedValue == null ||
                    string.IsNullOrWhiteSpace(SerialDriverNumber.Text) ||
                    string.IsNullOrWhiteSpace(NumberDriver.Text) ||
                    InspireDateLicense.SelectedDate == null ||
                    string.IsNullOrWhiteSpace(SerialPassport.Text) ||
                    string.IsNullOrWhiteSpace(NumberPassport.Text) ||
                    InspireDatePassport.SelectedDate == null)
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!FIORegex.IsMatch(FullName.Text))
                {
                    MessageBox.Show("ФИО может содержать только русские и английские буквы, пробелы и дефисы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!SerialDriverNumberRegex.IsMatch(SerialDriverNumber.Text))
                {
                    MessageBox.Show("Серия водительского удостоверения должна содержать 2 английские буквы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!NumberDriverRegex.IsMatch(NumberDriver.Text))
                {
                    MessageBox.Show("Номер водительского удостоверения должен содержать 6 цифр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!SerialPassportRegex.IsMatch(SerialPassport.Text))
                {
                    MessageBox.Show("Серия паспорта должна содержать 2 цифры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!NumberPassportRegex.IsMatch(NumberPassport.Text))
                {
                    MessageBox.Show("Номер паспорта должен содержать 10 цифр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DateBirth.SelectedDate == null || DateTime.Now.Year - DateBirth.SelectedDate.Value.Year < 18)
                {
                    MessageBox.Show("Пользователю должно быть не менее 18 лет!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int currentUserID = ((App)Application.Current).CurrentUserID;

                if (currentUserID == 0)
                {
                    MessageBox.Show("Ошибка: не найден текущий пользователь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var fioParts = FullName.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (fioParts.Length < 2 || fioParts.Length > 3)
                {
                    MessageBox.Show("Введите ФИО в формате: Фамилия Имя Отчество (при наличии).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string lastName = fioParts[0];
                string firstName = fioParts[1];
                string secondName = fioParts.Length == 3 ? fioParts[2] : "";

                var existingDriver = db.drivers.FirstOrDefault(d => d.user_id == currentUserID);
                if (existingDriver != null)
                {
                    MessageBox.Show("Пользователь уже зарегистрирован как водитель.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newDriver = new driver
                {
                    user_id = currentUserID,
                    first_name = firstName,
                    last_name = lastName,
                    second_name = secondName,
                    birth_date = DateBirth.SelectedDate.Value,
                    region_id = (int)RegionBox.SelectedValue,
                    driving_license_series = SerialDriverNumber.Text,
                    driving_license_number = NumberDriver.Text,
                    license_issue_date = InspireDateLicense.SelectedDate.Value,
                    passport_series = SerialPassport.Text,
                    passport_number = NumberPassport.Text,
                    passport_issue_date = InspireDatePassport.SelectedDate.Value
                };

                db.drivers.Add(newDriver);
                db.SaveChanges();

                FrameManager.MainFrame.Navigate(new bidcar());
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}