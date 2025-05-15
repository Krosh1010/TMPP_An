using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.AccessHandlers;
using Domain.Entities;
using App.Abstraction;

namespace App.Services
{
    public class EmployeeAccessService : IEmployeeAccessService
    {
        private readonly EmployeeAccessHandler _chainHead;

        public EmployeeAccessService()
        {
            var hr = new HRAccessHandler();
            var manager = new ManagerAccessHandler();
            hr.SetNext(manager);
            _chainHead = hr;
        }

        public List<Employee> GetAccessibleEmployees(User user, List<Employee> employees)
            => _chainHead.Handle(user, employees);
    }
}
