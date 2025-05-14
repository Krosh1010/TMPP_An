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
    public class EmployeeAppService: IEmployeeServices
    {
        private readonly EmployeeRepository _repo;
        private readonly List<Employee> _employees;
        private readonly List<IEmployeeObserver> _observers = new();


        public EmployeeAppService()
        {
            _repo = EmployeeRepository.Instance;
            _employees = _repo.LoadAll();
        }

        public List<Employee> GetEmployees()
        {
            return _employees;
        }

        public void AddEmployee(string name, string role, string team)
        {
            var builder = new EmployeeBuilder();
            builder.SetName(name);
            builder.SetRole(role);
            builder.SetTeam(team);
            builder.SetHireDate(DateTime.Now);

            var employee = builder.Build();
            _employees.Add(employee);
            _repo.SaveAll(_employees);

            NotifyObservers("Add", employee);

            NotificationService.Instance.Notify("Angajat adăugat cu succes");
        }

        public void RemoveEmployee(Employee employee)
        {
            _employees.Remove(employee);
            _repo.SaveAll(_employees);

            NotifyObservers("Remove", employee);

            NotificationService.Instance.Notify("Angajat șters cu succes");

        }

        public void UpdateEmployee(Employee updatedEmployee)
        {
            var index = _employees.FindIndex(emp => emp.HireDate == updatedEmployee.HireDate
                && emp.Name == updatedEmployee.Name); // simplu identificator
            if (index >= 0)
            {
                _employees[index] = updatedEmployee;
                _repo.SaveAll(_employees);
            }

            NotifyObservers("Edit", updatedEmployee);

            NotificationService.Instance.Notify("Angajat editat cu succes");
        }

        public void RegisterObserver(IEmployeeObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnregisterObserver(IEmployeeObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        private void NotifyObservers(string action, Employee employee)
        {
            foreach (var observer in _observers)
            {
                observer.OnEmployeeChanged(action, employee);
            }
        }


    }
}