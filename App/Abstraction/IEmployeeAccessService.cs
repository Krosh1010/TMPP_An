using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Abstraction
{
    public interface IEmployeeAccessService
    {
        List<Employee> GetAccessibleEmployees(User currentUser, List<Employee> employees);
    }
}
