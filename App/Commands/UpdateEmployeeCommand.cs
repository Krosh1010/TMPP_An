using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Commands
{
    public class UpdateEmployeeCommand : ICommand
    {
        private readonly IEmployeeServices _service;
        private readonly User _currentUser;
        private Employee _oldEmployee;
        private Employee _newEmployee;
        public UpdateEmployeeCommand(IEmployeeServices service, User currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public void Execute(Employee employee)
        {
            _oldEmployee = _service.GetEmployeesForUser(_currentUser)
                .FirstOrDefault(e => e.HireDate == employee.HireDate && e.Name == employee.Name);

            _newEmployee = employee;

            _service.UpdateEmployee(employee);
        }

        public void Undo()
        {
            if (_oldEmployee != null)
            {
                _service.UpdateEmployee(_oldEmployee);
            }
        }

    }

}
