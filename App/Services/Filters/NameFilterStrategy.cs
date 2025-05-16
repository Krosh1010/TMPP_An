using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Services.Filters
{
    public class NameFilterStrategy : IEmployeeFilterStrategy
    {
        public List<Employee> Filter(List<Employee> employees, string criteria)
        {
            return employees
                .Where(e => e.Name.Contains(criteria, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
