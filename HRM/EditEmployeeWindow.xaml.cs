using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Domain.Entities;

namespace HRM
{
    public partial class EditEmployeeWindow : Window
    {
        public string EmployeeName => NameTextBox.Text.Trim();
        public string EmployeeRole => (RoleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
        public string EmployeeTeam => (TeamComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            NameTextBox.Text = employee.Name;
            RoleComboBox.SelectedItem = RoleComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(i => (string)i.Content == employee.Role);
            TeamComboBox.SelectedItem = TeamComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(i => (string)i.Content == employee.Team);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmployeeName) || string.IsNullOrWhiteSpace(EmployeeRole))
            {
                MessageBox.Show("Completează numele și rolul!");
                return;
            }
            if (EmployeeRole != "HR" && string.IsNullOrWhiteSpace(EmployeeTeam))
            {
                MessageBox.Show("Selectează echipa!");
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var selectedRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
    if (selectedRole == "HR")
    {
        TeamComboBox.IsEnabled = false;
        TeamComboBox.SelectedIndex = -1;
    }
    else
    {
        TeamComboBox.IsEnabled = true;
    }
}
    }
}
