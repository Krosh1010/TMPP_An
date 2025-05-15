using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Observers;
using App.Abstraction;

namespace App.Services
{
    public class EmployeeNotifierService : IEmployeeNotifierService
    {
        private readonly List<IEmployeeObserver> _observers = new();

        public void RegisterObserver(IEmployeeObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnregisterObserver(IEmployeeObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        public void NotifyObservers(string action, Employee employee)
        {
            foreach (var observer in _observers)
            {
                observer.OnEmployeeChanged(action, employee);
            }
        }
    }
}
