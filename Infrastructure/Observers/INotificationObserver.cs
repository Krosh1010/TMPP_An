using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Observers
{
    public interface INotificationObserver
    {
        void OnNotification(string message, bool isError = false);
    }
}
