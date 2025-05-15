using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;
using Infrastructure.Repositories;

namespace App.Services
{
    public class EmployeeRepositoryAdapter : IEmployeeRepository
    {
        private readonly EmployeeRepository _repo = EmployeeRepository.Instance;

        public List<Employee> LoadAll() => _repo.LoadAll();
        public void SaveAll(List<Employee> employees) => _repo.SaveAll(employees);
    }
}
