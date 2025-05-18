using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class ManagerHrRepository 
    {
        private readonly string _filePath = "manager.hr.json";

        public List<Employee> LoadAll()
        {
            if (!File.Exists(_filePath))
                return new List<Employee>();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
        }

        public void SaveAll(List<Employee> employees)
        {
            // Filtrează doar Managerii și HR
            var managersAndHr = employees
                .Where(e => e.Role == "Manager" || e.Role == "HR")
                .ToList();

            File.WriteAllText(_filePath, JsonSerializer.Serialize(managersAndHr, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
