using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Builders;
using Domain.Entities;
using Domain.Factories;
using Infrastructure.Repositories;
using Domain.Observers;
using App.Abstraction;
using System.Collections.ObjectModel;

namespace App.Services
{
    public class EmployeeAppService : IEmployeeServices
    {
        private readonly IEmployeeRepository _repo;
        private readonly List<Employee> _employees;
        private readonly IEmployeeBuilderService _builderService;
        private readonly IEmployeeNotifierService _notifier;
        private readonly IEmployeeAccessService _accessService;
        private readonly INotificationService _notificationService;
        private readonly ManagerHrRepository _managerHrRepo;

        public EmployeeAppService(
            IEmployeeRepository repo,
            IEmployeeBuilderService builderService,
             IEmployeeNotifierService notifier,
            IEmployeeAccessService accessService,
            INotificationService notificationService,
            ManagerHrRepository managerHrRepo)
        {
            _repo = repo;
            _employees = _repo.LoadAll();
            _builderService = builderService;
            _notifier = notifier;
            _accessService = accessService;
            _notificationService = notificationService;
            _managerHrRepo = managerHrRepo;
        }

        public List<Employee> GetEmployeesForUser(User currentUser)
        {
            return _accessService.GetAccessibleEmployees(currentUser, _employees);
        }

        public void AddEmployee(string name, string role, string team)
        {
            var employee = _builderService.BuildEmployee(name, role, team, DateTime.Now);
            _employees.Add(employee);
            _repo.SaveAll(_employees);
            _managerHrRepo.SaveAll(_employees);

            _notifier.NotifyObservers("Add", employee);
            _notificationService.Notify("Angajat adăugat cu succes");
        }

        public void RemoveEmployee(Employee employee)
        {
            _employees.Remove(employee);
            _repo.SaveAll(_employees);
            _managerHrRepo.SaveAll(_employees);

            _notifier.NotifyObservers("Remove", employee);
            _notificationService.Notify("Angajat șters cu succes");
        }

        public void UpdateEmployee(Employee updatedEmployee)
        {
            var index = _employees.FindIndex(emp =>
                emp.HireDate == updatedEmployee.HireDate &&
                emp.Name == updatedEmployee.Name);
            if (index >= 0)
            {
                _employees[index] = updatedEmployee;
                _repo.SaveAll(_employees);
                _managerHrRepo.SaveAll(_employees);
            }

            _notifier.NotifyObservers("Edit", updatedEmployee);
            _notificationService.Notify("Angajat editat cu succes");
        }

        public void RegisterObserver(IEmployeeObserver observer)
        {
            _notifier.RegisterObserver(observer);
        }

        public void UnregisterObserver(IEmployeeObserver observer)
        {
            _notifier.UnregisterObserver(observer);
        }
    }
}