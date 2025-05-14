using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Builders
{
    public interface IEmployeeBuilder
    {
        void SetName(string name);
        void SetRole(string role);
        void SetTeam(string team);
        void SetHireDate(DateTime date);
        Employee Build();
    }
}

