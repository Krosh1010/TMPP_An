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
        private readonly EmployeeBuilderService _builderService;
        private readonly EmployeeNotifierService _notifier;

        public EmployeeAppService(IEmployeeRepository repo)
        {
            _repo = repo;
            _employees = _repo.LoadAll();
            _builderService = new EmployeeBuilderService();
            _notifier = new EmployeeNotifierService();
        }

        public List<Employee> GetEmployees() => _employees;

        public void AddEmployee(string name, string role, string team)
        {
            var employee = _builderService.BuildEmployee(name, role, team);
            _employees.Add(employee);
            _repo.SaveAll(_employees);

            _notifier.NotifyObservers("Add", employee);
            NotificationService.Instance.Notify("Angajat adăugat cu succes");
        }

        public void RemoveEmployee(Employee employee)
        {
            _employees.Remove(employee);
            _repo.SaveAll(_employees);

            _notifier.NotifyObservers("Remove", employee);
            NotificationService.Instance.Notify("Angajat șters cu succes");
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
            }

            _notifier.NotifyObservers("Edit", updatedEmployee);
            NotificationService.Instance.Notify("Angajat editat cu succes");
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