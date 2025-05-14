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
        private Employee _oldEmployee;
        private Employee _newEmployee;
        public UpdateEmployeeCommand(IEmployeeServices service)
        {
            _service = service;
        }

        public void Execute(Employee employee)
        {
            _oldEmployee = _service.GetEmployees()
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
