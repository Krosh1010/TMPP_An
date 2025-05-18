using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using App.Abstraction;
using App.Commands;
using App.Services.Filters;
using Domain.Entities;
using HRM.Services;

namespace HRM.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IEmployeeServices _service;
        private readonly CommandInvoker _commandInvoker = new CommandInvoker();
        private readonly EmployeeFilterContext _filterContext = new EmployeeFilterContext();
        private readonly User _currentUser;
        private readonly WindowResizeService _resizeService;
        private readonly Action _closeWindowAction;



        public ObservableCollection<Employee> EmployeeList { get; }
        public System.Windows.Input.ICommand AddEmployeeCommand { get; }
        public System.Windows.Input.ICommand EditEmployeeCommand { get; }
        public System.Windows.Input.ICommand DeleteEmployeeCommand { get; }
        public System.Windows.Input.ICommand UndoCommand { get; }
        public System.Windows.Input.ICommand FilterCommand { get; }
        public System.Windows.Input.ICommand LogoutCommand { get; }
        public System.Windows.Input.ICommand CancelDeleteCommand { get; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        private string _filterType;
        public string FilterType
        {
            get => _filterType;
            set => SetProperty(ref _filterType, value);
        }

        private string _filterCriteria;
        public string FilterCriteria
        {
            get => _filterCriteria;
            set => SetProperty(ref _filterCriteria, value);
        }

        public MainWindowViewModel(IEmployeeServices service, User currentUser, Action closeWindowAction)
        {
            _service = service;
            _currentUser = currentUser;
            _closeWindowAction = closeWindowAction;
            EmployeeList = new ObservableCollection<Employee>(_service.GetEmployeesForUser(_currentUser));

            AddEmployeeCommand = new RelayCommand(AddEmployee);
            EditEmployeeCommand = new RelayCommand(EditEmployee, () => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee, () => SelectedEmployee != null);
            UndoCommand = new RelayCommand(Undo);
            FilterCommand = new RelayCommand(FilterEmployees);
            LogoutCommand = new RelayCommand(Logout);
            CancelDeleteCommand = new RelayCommand(CancelDelete);
        }

        private void CancelDelete()
        {
        SelectedEmployee = null;
        }   
        private void AddEmployee()
        {
            var addWindow = new AddEmployeeWindow();
            addWindow.Owner = App.Current.MainWindow;
            if (addWindow.ShowDialog() == true)
            {
                var addCommand = new AddEmployeeCommand(
                    _service,
                    addWindow.EmployeeName,
                    addWindow.EmployeeRole,
                    addWindow.EmployeeTeam,
                    _currentUser
                );
                _commandInvoker.SetCommand(addCommand);
                _commandInvoker.ExecuteCommand(null);
                RefreshEmployees();
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;
            var editWindow = new EditEmployeeWindow(SelectedEmployee);
            editWindow.Owner = App.Current.MainWindow;
            if (editWindow.ShowDialog() == true)
            {
                var updatedEmployee = new Employee
                {
                    Id = SelectedEmployee.Id,
                    Name = editWindow.EmployeeName,
                    Role = editWindow.EmployeeRole,
                    Team = editWindow.EmployeeTeam,
                    HireDate = SelectedEmployee.HireDate,
                    StateName = editWindow.EmployeeStateName
                };

                var editCommand = new UpdateEmployeeCommand(_service, _currentUser);
                _commandInvoker.SetCommand(editCommand);
                _commandInvoker.ExecuteCommand(updatedEmployee);
                RefreshEmployees();
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null) return;
            var result = System.Windows.MessageBox.Show(
                $"Sigur vrei să ștergi angajatul {SelectedEmployee.Name}?",
                "Confirmare ștergere",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                var deleteCommand = new DeleteEmployeeCommand(_service);
                _commandInvoker.SetCommand(deleteCommand);
                _commandInvoker.ExecuteCommand(SelectedEmployee);
                RefreshEmployees();
            }
        }

        private void Undo()
        {
            _commandInvoker.UndoLastCommand();
            RefreshEmployees();
        }

        public void FilterEmployees()
        {
            var allEmployees = _service.GetEmployeesForUser(_currentUser);

            // Dacă nu ai criteriu sau tip de filtru, afișează toți angajații
            if (string.IsNullOrWhiteSpace(FilterType) || string.IsNullOrWhiteSpace(FilterCriteria))
            {
                EmployeeList.Clear();
                foreach (var emp in allEmployees)
                    EmployeeList.Add(emp);
                return;
            }

            // Setează strategia de filtrare pe baza FilterType
            IEmployeeFilterStrategy strategy = null;
            IEnumerable<Employee> filtered = allEmployees;

            switch (FilterType)
            {
                case "Nume":
                    strategy = new NameFilterStrategy();
                    break;
                case "Rol":
                    strategy = new RoleFilterStrategy();
                    break;
                case "Echipa":
                    strategy = new TeamFilterStrategy();
                    // Elimină angajații cu Team == null înainte de filtrare
                    filtered = allEmployees.Where(e => e.Team != null);
                    break;
            }
            _filterContext.SetStrategy(strategy);

            if (strategy != null)
            {
                var safeCriteria = FilterCriteria ?? string.Empty;
                filtered = _filterContext.Filter(filtered.ToList(), safeCriteria); // <-- adaugă .ToList()
            }

            EmployeeList.Clear();
            foreach (var emp in filtered)
                EmployeeList.Add(emp);
        }

        private void RefreshEmployees()
        {
            EmployeeList.Clear();
            foreach (var emp in _service.GetEmployeesForUser(_currentUser))
                EmployeeList.Add(emp);
        }

        private void Logout()
        {
            // Deschide fereastra de login dacă vrei
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // Închide fereastra curentă
            _closeWindowAction?.Invoke();
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeService.HandleWindowResize(e);
        }
    }
}