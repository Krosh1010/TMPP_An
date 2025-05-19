using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Messages
{
    public class EmployeeAddedMessage
    {
        public Employee Employee { get; set; }
        public EmployeeAddedMessage(Employee employee)
        {
            Employee = employee;
        }
    }
}
