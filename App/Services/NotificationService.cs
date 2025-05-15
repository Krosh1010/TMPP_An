using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Observers;
using App.Abstraction;

namespace App.Services
{
    public class NotificationService : INotificationService
    {
        private static NotificationService _instance;
        private readonly List<INotificationObserver> _observers = new();

        private NotificationService() { }

        public static NotificationService Instance => _instance ??= new NotificationService();

        public void Subscribe(INotificationObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Unsubscribe(INotificationObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        public void Notify(string message, bool isError = false)
        {
            foreach (var observer in _observers)
            {
                observer.OnNotification(message, isError);
            }
        }
    }
}
