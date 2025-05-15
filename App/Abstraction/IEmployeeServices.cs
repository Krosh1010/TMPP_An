using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Observers;
namespace App.Abstraction
{
    public interface IEmployeeServices 
    {
        List<Employee> GetEmployeesForUser(User currentUser);
        void AddEmployee(string name, string role, string team);
        void RemoveEmployee(Employee employee);
        void UpdateEmployee(Employee updatedEmployee);
        void RegisterObserver(IEmployeeObserver observer);
        void UnregisterObserver(IEmployeeObserver observer);

    }
}
