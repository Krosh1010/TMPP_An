using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Messages
{
    public class EmployeeDeletedMessage
    {
        public Employee Employee { get; set; }
        public EmployeeDeletedMessage(Employee employee)
        {
            Employee = employee;
        }
    }
}
