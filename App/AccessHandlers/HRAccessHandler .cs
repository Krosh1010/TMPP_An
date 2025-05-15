using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.AccessHandlers
{
    public class HRAccessHandler : EmployeeAccessHandler
    {
        public override List<Employee> Handle(User user, List<Employee> employees)
        {
            if (user.Role == "HR")
                return employees; // HR vede tot
            return next?.Handle(user, employees) ?? new List<Employee>();
        }
    }
}
