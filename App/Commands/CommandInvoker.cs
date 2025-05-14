using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace App.Commands
{
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _history = new();
        private ICommand _command;

        public void SetCommand(ICommand command)
        {
            _command = command; // Trebuie să salvezi comanda aici!
        }

        public void ExecuteCommand(Employee employee)
        {
            _command?.Execute(employee);
            if (_command != null)
                _history.Push(_command);
        }

        public void UndoLastCommand()
        {
            if (_history.Any())
            {
                var lastCommand = _history.Pop();
                lastCommand.Undo();
            }
        }
    }


}
