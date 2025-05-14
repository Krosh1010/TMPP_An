using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using App.Abstraction;
using App.Commands;
using App.Services;
using Domain.Entities;
using HRM.Observers;
using Infrastructure.Observers;

namespace HRM
{
    public partial class MainWindow : Window, INotificationObserver
    {
        private IEmployeeServices _service = new EmployeeAppService();
        private ObservableCollection<Employee> _employeeList;
        private Employee _selectedEmployeeForEdit;
        private readonly CommandInvoker _commandInvoker = new CommandInvoker();

        public MainWindow()
        {
            InitializeComponent();
            _employeeList = new ObservableCollection<Employee>(_service.GetEmployees());
            EmployeeDataGrid.ItemsSource = _employeeList;
            var logger = new EmployeeLogger();
            _service.RegisterObserver(logger);
            var uiUpdater = new EmployeeUIUpdater(_employeeList);
            _service.RegisterObserver(uiUpdater);
            NotificationService.Instance.Subscribe(this);
        }

        public void OnNotification(string message, bool isError = false)
        {
            MessageBox.Show(message, isError ? "Eroare" : "Info", MessageBoxButton.OK,
                isError ? MessageBoxImage.Error : MessageBoxImage.Information);
        }

        protected override void OnClosed(EventArgs e)
        {
            NotificationService.Instance.Unsubscribe(this);
            base.OnClosed(e);
        }

        private void ShowAddButton_Click(object sender, RoutedEventArgs e)
        {
            InputPanel.Visibility = Visibility.Visible;
            DeletePanel.Visibility = Visibility.Collapsed;
            ShowAddButton.Visibility = Visibility.Collapsed;
            EmployeeDataGrid.SelectedItem = null;
            NameTextBox.Focus();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string team = (TeamComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(team))
            {
                MessageBox.Show("Completează numele, rolul și echipa!");
                return;
            }

            if (_selectedEmployeeForEdit != null)
            {
                // Modificăm angajatul existent
                _selectedEmployeeForEdit.Name = name;
                _selectedEmployeeForEdit.Role = role;
                _selectedEmployeeForEdit.Team = team;

                // Refresh DataGrid
                EmployeeDataGrid.Items.Refresh();

                // Salvează în fișier
                var updateCommand = new UpdateEmployeeCommand(_service);
                _commandInvoker.SetCommand(updateCommand);
                _commandInvoker.ExecuteCommand(_selectedEmployeeForEdit); // Trimite angajatul editat

                _selectedEmployeeForEdit = null;
            }
            else
            {
                // Folosește AddEmployeeCommand
                var addCommand = new AddEmployeeCommand(_service, name, role, team);
                _commandInvoker.SetCommand(addCommand);
                _commandInvoker.ExecuteCommand(null);
            }

            // Resetare UI
            NameTextBox.Text = "";
            RoleComboBox.SelectedIndex = -1;
            TeamComboBox.SelectedIndex = -1;
            InputPanel.Visibility = Visibility.Collapsed;
            ShowAddButton.Visibility = Visibility.Visible;

            // Resetare buton
            ((Button)sender).Content = "Adaugă";
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Anulare și revenire la starea inițială
            NameTextBox.Text = "";
            RoleComboBox.SelectedIndex = -1;
            TeamComboBox.SelectedIndex = -1;
            InputPanel.Visibility = Visibility.Collapsed;
            ShowAddButton.Visibility = Visibility.Visible;
        }

        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem != null)
            {
                // Afișează panoul de ștergere
                InputPanel.Visibility = Visibility.Collapsed;
                DeletePanel.Visibility = Visibility.Visible;
                ShowAddButton.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelDelete_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDataGrid.SelectedItem = null;
            DeletePanel.Visibility = Visibility.Collapsed;
            ShowAddButton.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = EmployeeDataGrid.SelectedItem as Employee;
            if (selected != null)
            {
                var deleteCommand = new DeleteEmployeeCommand(_service);
                _commandInvoker.SetCommand(deleteCommand);
                _commandInvoker.ExecuteCommand(selected);

                EmployeeDataGrid.SelectedItem = null;
                DeletePanel.Visibility = Visibility.Collapsed;
                ShowAddButton.Visibility = Visibility.Visible;
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            _selectedEmployeeForEdit = EmployeeDataGrid.SelectedItem as Employee;
            if (_selectedEmployeeForEdit == null)
                return;

            // Populează câmpurile
            NameTextBox.Text = _selectedEmployeeForEdit.Name;
            NameTextBox.Foreground = Brushes.Black;
            RoleComboBox.SelectedItem = RoleComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _selectedEmployeeForEdit.Role);

            TeamComboBox.SelectedItem = TeamComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _selectedEmployeeForEdit.Team);

            // Arată panelul de input
            InputPanel.Visibility = Visibility.Visible;
            DeletePanel.Visibility = Visibility.Collapsed;
            ShowAddButton.Visibility = Visibility.Collapsed;

            // Schimbă textul butonului de adăugare
            ((Button)InputPanel.Children
                .OfType<StackPanel>()
                .Last()
                .Children[0]).Content = "Salvează";
        }

        // Placeholder text handling for NameTextBox
        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "Introdu nume")
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameTextBox.Text = "Introdu nume";
                NameTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            _commandInvoker.UndoLastCommand();
            EmployeeDataGrid.Items.Refresh();
        }
    }
}
