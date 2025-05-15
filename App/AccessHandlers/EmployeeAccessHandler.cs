using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.AccessHandlers
{
    public abstract class EmployeeAccessHandler
    {
        protected EmployeeAccessHandler next;
        public void SetNext(EmployeeAccessHandler handler) => next = handler;
        public abstract List<Employee> Handle(User user, List<Employee> employees);
    }
}
