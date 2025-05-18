using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public abstract class EmployeeState
    {
        public abstract string Name { get; }
    }

    public class ActiveState : EmployeeState
    {
        public ActiveState() { }
        public override string Name => "Activ";
    }
    public class OnVacationState : EmployeeState
    {
        public OnVacationState() { }
        public override string Name => "În concediu";
    }
}
