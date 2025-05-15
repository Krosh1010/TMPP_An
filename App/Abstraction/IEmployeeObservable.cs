using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Observers;

namespace App.Abstraction
{
    public interface IEmployeeObservable
    {
        void RegisterObserver(IEmployeeObserver observer);
        void UnregisterObserver(IEmployeeObserver observer);
    }
}
