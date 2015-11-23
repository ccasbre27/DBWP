using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.ViewModels
{
    public class ActionCommand : ICommand
    {
        Action action;

        public ActionCommand(Action pAction)
        {
            action = pAction;
        }
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

       public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}
