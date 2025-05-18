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

namespace HRM
{
    public partial class AddEmployeeWindow : Window
    {
        public string EmployeeName => NameTextBox.Text.Trim();
        public string EmployeeRole => (RoleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
        public string EmployeeTeam => (TeamComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

        public AddEmployeeWindow()
        {
            InitializeComponent();
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
        TeamComboBox.SelectedIndex = -1; // Deselectează orice echipă
    }
    else
    {
        TeamComboBox.IsEnabled = true;
    }
}
    }
}
