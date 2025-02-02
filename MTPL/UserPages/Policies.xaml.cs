﻿using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MTPL.UserPages
{

    public partial class Policies : Page
    {
        private readonly MTPLEntities db;
        public Policies()
        {
            InitializeComponent();
            db = new MTPLEntities();
            DataGridPolicies.ItemsSource = db.policies.ToList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var policyToDelete = DataGridPolicies.SelectedItems.Cast<policy>().Select(p => p.policy_id).ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {policyToDelete.Count()} полисов?", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var PoliciesToDelete = db.policies.Where(p => policyToDelete.Contains(p.policy_id)).ToList();
                    db.policies.RemoveRange(PoliciesToDelete);
                    db.SaveChanges();
                    DataGridPolicies.ItemsSource = db.policies.ToList();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(DataGridPolicies.SelectedItem is policy selectedPolicy)
            {
                FrameManager.MainFrame.Navigate(new InfoPolicy(selectedPolicy));
            }
            else
            {
                MessageBox.Show("Выберите полис для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadData(string searchQuery = "")
        {
            var policies = db.policies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                policies = policies.Where(p =>
                    p.driver.last_name.Contains(searchQuery) ||
                    p.driver.first_name.Contains(searchQuery) ||
                    p.driver.second_name.Contains(searchQuery) ||
                    (p.driver.last_name + " " + p.driver.first_name + " " + p.driver.second_name).Contains(searchQuery)
                );
            }

            DataGridPolicies.ItemsSource = policies.ToList();
        }


        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(TxtSearch.Text);
        }
    }
}
