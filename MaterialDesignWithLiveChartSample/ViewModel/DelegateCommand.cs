using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object>? executeAction;
        private readonly Func<object, bool>? canExecuteAction;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute=null!)
        {
            executeAction = execute;
            canExecuteAction = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => canExecuteAction?.Invoke(parameter!) ?? true;
        
        public void Execute(object? parameter) => executeAction?.Invoke(parameter!);

        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
