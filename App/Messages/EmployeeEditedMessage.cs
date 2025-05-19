using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Messages
{
    public class EmployeeEditedMessage
    {
        public Employee Employee { get; set; }
        public EmployeeEditedMessage(Employee employee)
        {
            Employee = employee;
        }
    }
}
