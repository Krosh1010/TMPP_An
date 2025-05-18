using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.States;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }
        public DateTime HireDate { get; set; }

        [JsonIgnore]
        public IEmployeeState State { get; set; } = new ActiveState();

        // Proprietate pentru serializare/deserializare stării
        public string StateName
        {
            get => State?.Name ?? "Activ";
            set
            {
                State = value switch
                {
                    "Activ" => new ActiveState(),
                    "În concediu" => new OnVacationState(),
                    // Adaugă aici alte stări dacă există
                    _ => State // nu schimba dacă nu recunoaște starea
                };
            }
        }

        public Employee()
        {
            Id = Guid.NewGuid();
        }

        public Employee(string name, string role, string team, DateTime hireDate, IEmployeeState state = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Role = role;
            Team = team;
            HireDate = hireDate;
            State = state ?? new ActiveState();
        }
    }
}