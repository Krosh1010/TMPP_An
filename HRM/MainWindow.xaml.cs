using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using App.Abstraction;
using App.Commands;
using App.Services;
using App.Messages;
using App.Services.Mediator;
using Domain.Entities;
using HRM.Observers;
using Infrastructure.Observers;
using App.Services.Filters;
using Infrastructure.Repositories;
using HRM.Services;

namespace HRM
{
    public partial class MainWindow : Window, INotificationObserver
    {
        private IEmployeeServices _service;
        private ObservableCollection<Employee> _employeeList;
        private Employee _selectedEmployeeForEdit;
        private readonly CommandInvoker _commandInvoker = new CommandInvoker();
        private readonly EmployeeFilterContext _filterContext = new EmployeeFilterContext();
        private readonly WindowResizeService _resizeService;
        

        private User _currentUser;
        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _service = new EmployeeAppService(
              new EmployeeRepositoryAdapter(),
              new EmployeeBuilderService(),
            new EmployeeNotifierService(),
            new EmployeeAccessService(),
            NotificationService.Instance,
            new ManagerHrRepository()
    );
            MediatorProvider.Instance.Subscribe<EmployeeAddedMessage>(OnEmployeeAdded);
            MediatorProvider.Instance.Subscribe<EmployeeEditedMessage>(OnEmployeeEdited);
            MediatorProvider.Instance.Subscribe<EmployeeDeletedMessage>(OnEmployeeDeleted);
            _employeeList = new ObservableCollection<Employee>(_service.GetEmployeesForUser(_currentUser));
            EmployeeDataGrid.ItemsSource = _employeeList;
            var logger = new EmployeeLogger();
            _service.RegisterObserver(logger);
            var uiUpdater = new EmployeeUIUpdater(_employeeList);
            _service.RegisterObserver(uiUpdater);
            NotificationService.Instance.Subscribe(this);

            _resizeService = new WindowResizeService(
                TitleText,
                ActionButtonsPanel,
                FilterPanel,
                ShowAddButton,
                UndoButton,
                LogoutButton);


            if (_currentUser.Role != "HR")
            {
                ShowAddButton.Visibility = Visibility.Collapsed;
                UndoButton.Visibility = Visibility.Collapsed;
                // Păstrează doar opțiunea "Nume" în filtrare pentru manageri
                var itemsToRemove = FilterTypeComboBox.Items
                    .OfType<ComboBoxItem>()
                    .Where(i => (string)i.Content != "Nume")
                    .ToList();

                foreach (var item in itemsToRemove)
                    FilterTypeComboBox.Items.Remove(item);

                FilterTypeComboBox.SelectedIndex = 0;
            }
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
            MediatorProvider.Instance.Unsubscribe<EmployeeAddedMessage>(OnEmployeeAdded);
            MediatorProvider.Instance.Unsubscribe<EmployeeEditedMessage>(OnEmployeeEdited);
            MediatorProvider.Instance.Unsubscribe<EmployeeDeletedMessage>(OnEmployeeDeleted);
        }

        private void ShowAddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEmployeeWindow();
            addWindow.Owner = this;
            if (addWindow.ShowDialog() == true)
            {
                // Folosește AddEmployeeCommand
                var addCommand = new AddEmployeeCommand(
                    _service,
                    addWindow.EmployeeName,
                    addWindow.EmployeeRole,
                    addWindow.EmployeeTeam,
                    _currentUser
                );
                _commandInvoker.SetCommand(addCommand);
                _commandInvoker.ExecuteCommand(null);
            }
        }

        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem != null)
            {
                // Permite meniul de ștergere doar pentru HR
                if (_currentUser.Role == "HR")
                {

                    DeletePanel.Visibility = Visibility.Visible;
                    ShowAddButton.Visibility = Visibility.Collapsed;
                    FilterPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Pentru manageri sau alte roluri, nu afișa nimic
                    EmployeeDataGrid.SelectedItem = null;
                    DeletePanel.Visibility = Visibility.Collapsed;

                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selected = EmployeeDataGrid.SelectedItem as Employee;
            if (selected != null)
            {
                var editWindow = new EditEmployeeWindow(selected);
                editWindow.Owner = this;
                if (editWindow.ShowDialog() == true)
                {
                    var updatedEmployee = new Employee
                    {
                        Id = selected.Id,
                        Name = editWindow.EmployeeName,
                        Role = editWindow.EmployeeRole,
                        Team = editWindow.EmployeeTeam,
                        HireDate = selected.HireDate,
                        StateName = editWindow.EmployeeStateName
                    };

                    var editCommand = new UpdateEmployeeCommand(_service, _currentUser);
                    _commandInvoker.SetCommand(editCommand);
                    _commandInvoker.ExecuteCommand(updatedEmployee);

                    EmployeeDataGrid.SelectedItem = null;
                    DeletePanel.Visibility = Visibility.Collapsed;
                    ShowAddButton.Visibility = Visibility.Visible;
                    FilterPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void CancelDelete_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDataGrid.SelectedItem = null;
            DeletePanel.Visibility = Visibility.Collapsed;
            ShowAddButton.Visibility = Visibility.Visible;
            FilterPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = EmployeeDataGrid.SelectedItem as Employee;
            if (selected != null)
            {
                var result = MessageBox.Show(
                    $"Sigur vrei să ștergi angajatul {selected.Name}?",
                    "Confirmare ștergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var deleteCommand = new DeleteEmployeeCommand(_service);
                    _commandInvoker.SetCommand(deleteCommand);
                    _commandInvoker.ExecuteCommand(selected);
                }

                EmployeeDataGrid.SelectedItem = null;
                DeletePanel.Visibility = Visibility.Collapsed;
                ShowAddButton.Visibility = Visibility.Visible;
                FilterPanel.Visibility = Visibility.Visible;
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            _commandInvoker.UndoLastCommand();
            EmployeeDataGrid.Items.Refresh();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Deschide fereastra de login
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // Închide fereastra curentă
            this.Close();
        }

        private void FilterEmployees(string filterType, string criteria)
        {
            switch (filterType)
            {
                case "Nume":
                    _filterContext.SetStrategy(new NameFilterStrategy());
                    break;
                case "Rol":
                    _filterContext.SetStrategy(new RoleFilterStrategy());
                    break;
                case "Echipa":
                    _filterContext.SetStrategy(new TeamFilterStrategy());
                    break;
                default:
                    _filterContext.SetStrategy(null);
                    break;
            }

            // Ia lista de angajați și setează Team la "Fără echipă" dacă e null
            var employees = _service.GetEmployeesForUser(_currentUser)
                .Select(emp =>
                {
                    if (emp.Team == null)
                        emp.Team = "Fără echipă";
                    return emp;
                }).ToList();

            var filtered = _filterContext.Filter(employees, criteria);
            _employeeList.Clear();
            foreach (var emp in filtered)
                _employeeList.Add(emp);
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterType = (FilterTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string criteria = FilterTextBox.Text.Trim();
            FilterEmployees(filterType, criteria);
        }

        private void OnEmployeeAdded(EmployeeAddedMessage msg)
        {
            if (!_employeeList.Any(e => e.Id == msg.Employee.Id))
                _employeeList.Add(msg.Employee);
        }

        private void OnEmployeeEdited(EmployeeEditedMessage msg)
        {
            var existing = _employeeList.FirstOrDefault(e => e.Id == msg.Employee.Id);
            if (existing != null)
            {
                var index = _employeeList.IndexOf(existing);
                _employeeList[index] = msg.Employee;
                EmployeeDataGrid.Items.Refresh();
            }
        }

        private void OnEmployeeDeleted(EmployeeDeletedMessage msg)
        {
            var toRemove = _employeeList.FirstOrDefault(e => e.Id == msg.Employee.Id);
            if (toRemove != null)
                _employeeList.Remove(toRemove);
        }

        //Style

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeService.HandleWindowResize(e);
        }

    }
}