using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Abstraction
{
    public interface IManagerHrRepository
    {
        List<Employee> LoadAll();
        void SaveAll(List<Employee> employees);
    }
}
