using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;

namespace App.Services.Mediator
{
    public class Mediator : IMediator
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public void Subscribe<TMessage>(Action<TMessage> handler)
        {
            var type = typeof(TMessage);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();
            _subscribers[type].Add(handler);
        }

        public void Unsubscribe<TMessage>(Action<TMessage> handler)
        {
            var type = typeof(TMessage);
            if (_subscribers.ContainsKey(type))
                _subscribers[type].Remove(handler);
        }

        public void Publish<TMessage>(TMessage message)
        {
            var type = typeof(TMessage);
            if (_subscribers.ContainsKey(type))
            {
                foreach (var handler in _subscribers[type])
                    ((Action<TMessage>)handler)?.Invoke(message);
            }
        }
    }
}
