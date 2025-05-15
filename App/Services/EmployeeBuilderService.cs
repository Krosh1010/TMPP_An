using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Builders;
using Domain.Entities;
using Domain.Factories;

namespace App.Services
{
    public class EmployeeBuilderService
    {
        public Employee BuildEmployee(string name, string role, string team)
        {
            var builder = new EmployeeBuilder();
            builder.SetName(name);
            builder.SetRole(role);
            builder.SetTeam(team);
            builder.SetHireDate(DateTime.Now);
            return builder.Build();
        }
    }
}
