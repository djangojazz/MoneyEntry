using System;     
using System.Windows.Input;

namespace Controls
{
  public sealed class DelegateCommand<T> : ICommand
  {
    private readonly Action<T> _execute;
    private readonly Predicate<T> _canExecute;

    public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
    {
      _execute = execute;
      _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      if (_canExecute == null)
        {
            return true;
        }
 
        return _canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
      _execute.Invoke((T)parameter);
    }

    public void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
