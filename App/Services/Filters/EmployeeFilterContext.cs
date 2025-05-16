using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Services.Filters
{
    public class EmployeeFilterContext
    {
        private IEmployeeFilterStrategy _strategy;

        public void SetStrategy(IEmployeeFilterStrategy strategy)
        {
            _strategy = strategy;
        }

        public List<Employee> Filter(List<Employee> employees, string criteria)
        {
            return _strategy?.Filter(employees, criteria) ?? employees;
        }
    }
}
