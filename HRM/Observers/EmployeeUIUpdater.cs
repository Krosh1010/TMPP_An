using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Observers;

namespace HRM.Observers
{
    public class EmployeeUIUpdater : IEmployeeObserver
    {
        private readonly ObservableCollection<Employee> _employees;

        public EmployeeUIUpdater(ObservableCollection<Employee> employees)
        {
            _employees = employees;
        }

        public void OnEmployeeChanged(string action, Employee employee)
        {
            switch (action)
            {
                case "Add":
                    _employees.Add(employee);
                    break;
                case "Edit":
                    var index = _employees.IndexOf(_employees.FirstOrDefault(e => e.Id == employee.Id));
                    if (index >= 0)
                    {
                        _employees[index] = employee;
                    }
                    break;
                case "Remove":
                    var toRemove = _employees.FirstOrDefault(e => e.Id == employee.Id);
                    if (toRemove != null)
                        _employees.Remove(toRemove);
                    break;
            }
        }
    }
}
