using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Commands
{
    public class AddEmployeeCommand : ICommand
    {
        private readonly IEmployeeServices _service;
        private Employee? _addedEmployee;
        // Accept employee details as parameters
        private readonly string _name;
        private readonly string _role;
        private readonly string _team;
        
        private readonly User _currentUser;

        public AddEmployeeCommand(IEmployeeServices service, string name, string role, string team, User currentUser)
        {
            _service = service;
            _name = name;
            _role = role;
            _team = team;
            _currentUser = currentUser;
        }

        public void Execute(Employee employee)
        {
            _service.AddEmployee(_name, _role, _team);
            _addedEmployee = _service.GetEmployeesForUser(_currentUser).Last(); // presupunem că este ultimul adăugat
        }

        public void Undo()
        {
            if (_addedEmployee != null)
            {
                _service.RemoveEmployee(_addedEmployee);
            }
        }
    }
}
