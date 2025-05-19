using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Abstraction
{
    public interface IMediator
    {
        void Subscribe<TMessage>(Action<TMessage> handler);
        void Unsubscribe<TMessage>(Action<TMessage> handler);
        void Publish<TMessage>(TMessage message);
    }
}
