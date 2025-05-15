using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.Builders
{
    public class EmployeeBuilder : IEmployeeBuilder
    {
        private Employee _employee;

        public EmployeeBuilder()
        {
            _employee = new Employee();
        }


        public void SetName(string name)
        {
            _employee.Name = name;
        }

        public void SetRole(string role)
        {
            _employee.Role = role;
        }

        public void SetTeam(string team)
        {
            _employee.Team = team;
        }

        public void SetHireDate(DateTime date)
        {
            _employee.HireDate = date;
        }

        public Employee Build()
        {
            return _employee;
        }
    }
}

