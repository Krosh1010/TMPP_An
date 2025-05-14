using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Commands
{
    public interface ICommand
    {
        void Execute(Employee employee);
        void Undo();
    }
}
