using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace EngineeringNotebook.Mvvm
{
  public class DelegateCommand : ICommand
  {
    private readonly Action _callback;
    private readonly Func<bool> _canExecute;

    public DelegateCommand(Action callback, Func<bool> canExecute = null)
    {
      _callback = callback;
      _canExecute = canExecute ?? (() => true);
    }

    public bool CanExecute(object parameter)
      => _canExecute.Invoke();

    public void Execute(object parameter)
      => _callback.Invoke();

    public event EventHandler CanExecuteChanged;

    public virtual void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  public class DelegateCommand<T> : ICommand
  {
    private readonly Action<T> _callback;
    private readonly Func<T, bool> _canExecute;

    public DelegateCommand(Action<T> callback, Func<T, bool> canExecute = null)
    {
      _callback = callback;
      _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
      => _canExecute == null || _canExecute.Invoke((T)parameter);

    public void Execute(object parameter)
      => _callback.Invoke((T)parameter);

    public event EventHandler CanExecuteChanged;

    public virtual void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}