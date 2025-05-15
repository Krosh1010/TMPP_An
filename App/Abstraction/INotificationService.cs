using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Abstraction
{
    public interface INotificationService
    {
        void Notify(string message, bool isError = false);
    }
}
