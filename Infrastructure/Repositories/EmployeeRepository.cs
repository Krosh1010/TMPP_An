using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Factories;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository
    {
        private static EmployeeRepository _instance;
        private static readonly object _lock = new object();

        private readonly string _filePath = "employees.json";

        private EmployeeRepository() { }

        public static EmployeeRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new EmployeeRepository();
                    return _instance;
                }
            }
        }

        public List<Employee> LoadAll()
        {
            if (!File.Exists(_filePath))
                return new List<Employee>();

            string json = File.ReadAllText(_filePath);
            var rawList = JsonSerializer.Deserialize<List<Employee>>(json);

            // Reconstruim cu factory, în funcție de Role
            var reconstructed = rawList.Select(emp =>
            {
                return emp.Role switch
                {
                    "HR" => EmployeeFactory.CreateHR(emp.Name, emp.Team),
                    "Manager" => EmployeeFactory.CreateManager(emp.Name, emp.Team),
                    "Developer" => EmployeeFactory.CreateDeveloper(emp.Name, emp.Team),
                    _ => emp // fallback
                };
            }).ToList();

            return reconstructed;
        }


        public void SaveAll(List<Employee> employees)
        {
            string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}