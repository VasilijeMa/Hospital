using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZdravoCorp.Core.Commands
{
    public abstract class BaseCommand:ICommand
    {
        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }
        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public abstract void Execute(object? parameter);

        public event EventHandler? CanExecuteChanged;
    }
}
