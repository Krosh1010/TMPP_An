using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Observers;

namespace App.Abstraction
{
    public interface IEmployeeNotifierService
    {
        void NotifyObservers(string action, Employee employee);
        void RegisterObserver(IEmployeeObserver observer);
        void UnregisterObserver(IEmployeeObserver observer);
    }
}
