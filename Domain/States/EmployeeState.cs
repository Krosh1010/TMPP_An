using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class ActiveState : IEmployeeState
    {
        public string Name => "Activ";
    }

    public class OnVacationState : IEmployeeState
    {
        public string Name => "În concediu";
    }
}
