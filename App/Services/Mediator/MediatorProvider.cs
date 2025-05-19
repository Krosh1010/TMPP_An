using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;

namespace App.Services.Mediator
{
    public static class MediatorProvider
    {
        public static IMediator Instance { get; } = new Mediator();
    }
}
