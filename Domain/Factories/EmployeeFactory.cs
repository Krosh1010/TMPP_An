using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Factories
{
    public static class EmployeeFactory
    {
        public static Employee CreateManager(string name, string team)
        {
            return new Employee(name, "Manager", team, DateTime.Now);
        }

        public static Employee CreateHR(string name, string team)
        {
            return new Employee(name, "HR" , team, DateTime.Now);
        }

        public static Employee CreateDeveloper(string name, string team)
        {
            return new Employee(name, "Developer" , team, DateTime.Now);
        }
    }
}