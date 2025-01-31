using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml.Permissions;

namespace MTPL.UserPages
{
    /// <summary>
    /// Логика взаимодействия для InfoPolicy.xaml
    /// </summary>
    public partial class InfoPolicy : Page
    {
        private readonly Regex insuranceRegex = new Regex(@"^[А-Яа-яA-Za-z\s\-]+$");
        private readonly MTPLEntities db;
        private policy currentPolicy;

        private readonly List<string> validOSAGOCompanies = new List<string>
    {
        "Согласие", "РЕСО", "Ингосстрах", "ВСК",
        "АльфаСтрахование", "Росгосстрах", "Тинькофф Страхование"
    };

        public InfoPolicy(policy selectedPolicy)
        {
            InitializeComponent();
            db = new MTPLEntities();
            currentPolicy = db.policies.FirstOrDefault(p => p.policy_id == selectedPolicy.policy_id);

            if (currentPolicy != null)
            {
                LoadPolicyData();
            }
        }

        private void LoadPolicyData()
        {
            PolicyNumber.Text = currentPolicy.policy_number;
            InsuranceCompany.Text = currentPolicy.insurance_company;
            IssueDate.SelectedDate = currentPolicy.issue_date;
            ExpirationDate.SelectedDate = currentPolicy.expiration_date;
            FIODriver.Text = currentPolicy.driver.FullName;
            CarDriver.Text = currentPolicy.car.FullCarInfo;
            DrivingLicenseSerial.Text = currentPolicy.driving_license_series;
            DrivingLicenseNumber.Text = currentPolicy.driving_license_number;
            Cost.Text = currentPolicy.cost.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (currentPolicy != null)
            {
                try
                {
                    string insuranceCompany = InsuranceCompany.Text.Trim();

                    
                    if (!insuranceRegex.IsMatch(insuranceCompany))
                    {
                        MessageBox.Show("Ошибка! В поле 'Страховая компания' можно вводить только русские или английские символы.",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                   
                    if (!validOSAGOCompanies.Contains(insuranceCompany))
                    {
                        MessageBox.Show("Ошибка! Страховая компания должна предоставлять ОСАГО.",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    
                    DateTime? issueDate = IssueDate.SelectedDate;
                    DateTime? expirationDate = ExpirationDate.SelectedDate;

                    if (issueDate == null || expirationDate == null)
                    {
                        MessageBox.Show("Ошибка! Даты начала и окончания действия полиса не могут быть пустыми.",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    
                    if (expirationDate < issueDate)
                    {
                        MessageBox.Show("Ошибка! Дата окончания полиса не может быть раньше даты начала.",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    
                    if (issueDate > DateTime.Now)
                    {
                        MessageBox.Show("Ошибка! Дата начала действия полиса не может быть в будущем.",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    
                    currentPolicy.insurance_company = insuranceCompany;
                    currentPolicy.issue_date = issueDate.Value;
                    currentPolicy.expiration_date = expirationDate.Value;


                    if (decimal.TryParse(Cost.Text, out decimal cost))
                        currentPolicy.cost = cost;
                    else
                    {
                        MessageBox.Show("Ошибка! В поле 'Конечная цена' должно быть число.",
                            "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    db.SaveChanges();
                    MessageBox.Show("Полис успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.Navigate(new Policies());
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}