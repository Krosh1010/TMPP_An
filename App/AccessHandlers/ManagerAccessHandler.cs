using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.AccessHandlers
{
    public class ManagerAccessHandler : EmployeeAccessHandler
    {
        public override List<Employee> Handle(User user, List<Employee> employees)
        {
            if (user.Role == "Manager")
                return employees.Where(e => e.Team == user.Team && e.Role == "Developer").ToList();
            return next?.Handle(user, employees) ?? new List<Employee>();
        }
    }
}
