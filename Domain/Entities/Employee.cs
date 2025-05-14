using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public string Team { get; set; }
        public DateTime HireDate { get; set; }

        public Employee() {
            Id = Guid.NewGuid();
        }

        public Employee(string name, string role,string team, DateTime hireDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Role = role;
            Team = team;
            HireDate = hireDate;
        }
    }
}