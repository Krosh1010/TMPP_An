using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Commands
{
    public class DeleteEmployeeCommand : ICommand
    {
        private readonly IEmployeeServices _service;
        private Employee _deletedEmployee;
        public DeleteEmployeeCommand(IEmployeeServices service)
        {
            _service = service;
        }

        public void Execute(Employee employee)
        {
            _deletedEmployee = employee;
            _service.RemoveEmployee(employee);
        }

        public void Undo()
        {
            if (_deletedEmployee != null)
            {
                _service.AddEmployee(_deletedEmployee.Name, _deletedEmployee.Role, _deletedEmployee.Team);
            }
        }
    }
}
