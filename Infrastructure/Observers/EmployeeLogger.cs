using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Observers;

namespace Infrastructure.Observers
{
    public class EmployeeLogger : IEmployeeObserver
    {
        public void OnEmployeeChanged(string action, Employee employee)
        {
            Console.WriteLine($"[{DateTime.Now}] {action} - {employee.Name}, {employee.Role}, {employee.Team}");
        }
    }
}
