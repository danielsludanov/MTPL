﻿using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MTPL.UserPages
{

    public partial class bidcar : Page
    {

        private static readonly Regex CarRegex = new Regex(@"^[a-zA-Zа-яА-Я]+$");
        private static readonly Regex yearManafactureRegex = new Regex(@"^\d{4}$");
        private static readonly Regex regNumberRegex = new Regex(@"^[a-zA-Z0-9]{8,}$");
        private static readonly Regex NumberRegex = new Regex(@"^\d{10}$");

        
        private readonly MTPLEntities db;
        public bool isNav = false;
        public bidcar()
        {
            InitializeComponent();
            db = new MTPLEntities();
            LoadCategories();
            LoadBrands();
        }

        private void LoadCategories()
        {
            var categories = db.car_categories.ToList();
            CategoryBox.ItemsSource = categories;
            CategoryBox.DisplayMemberPath = "category_name";
            CategoryBox.SelectedValuePath = "category_id";
        }

        private void LoadBrands()
        {
            var brands = db.car_brands.ToList();
            BrandCar.ItemsSource = brands;
            BrandCar.DisplayMemberPath = "brand_name";
            BrandCar.SelectedValuePath = "brand_id";
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!CarRegex.IsMatch(BrandCar.Text))
                {
                    MessageBox.Show("Модель автомобиля должна содержать только русские и английские буквы.");
                    return;
                }

                if (CategoryBox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите категорию автомобиля.");
                    return;
                }

                if (!CarRegex.IsMatch(ModelCar.Text))
                {
                    MessageBox.Show("Модель автомобиля должна содержать только русские и английские буквы.");
                    return;
                }

                if (yearManafactureRegex.IsMatch(YearManifacture.ToString()))
                {
                    MessageBox.Show("Год производства должен состоять из 4 цифр.");
                    return;
                }

                if (!regNumberRegex.IsMatch(RegNumber.Text))
                {
                    MessageBox.Show("Регистрационный номер должен быть алфавитно-цифровым и не менее 8 символов.");
                    return;
                }

                if (!NumberRegex.IsMatch(STSNumber.Text))
                {
                    MessageBox.Show("СТС номер должен содержать ровно 10 цифр.");
                    return;
                }

                if (!NumberRegex.IsMatch(PTSnumber.Text))
                {
                    MessageBox.Show("ПТС номер должен содержать ровно 10 цифр.");
                    return;
                }

                var app = (App)Application.Current;
                var driver = db.drivers.FirstOrDefault(d => d.user_id == app.CurrentUserID);
                if (driver == null)
                {
                    MessageBox.Show("Ошибка: Водитель не найден!");
                    return;
                }

                

                int brandId = (int)BrandCar.SelectedValue;

                var newCar = new car
                {
                    brand_id = brandId,
                    category_id = (int)CategoryBox.SelectedValue,
                    driver_id = driver.driver_id,
                    model = ModelCar.Text,
                    year_manufacture = int.Parse(YearManifacture.Text),
                    reg_number = RegNumber.Text,
                    stc_number = STSNumber.Text,
                    pts_number = PTSnumber.Text
                };

                db.cars.Add(newCar);
                db.SaveChanges();

                MessageBox.Show("Заявка успешно отправлена! Статус заявки вы можете увидеть на главном экране (проверить статус");
                
                // Переход на проверку статуса
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}