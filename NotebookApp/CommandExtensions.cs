using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EngineeringNotebook
{
  internal static class CommandExtensions
  {
    public static bool TryExecute<T>(this ICommand command, T argument)
    {
      if (command == null)
        return false;

      if (!command.CanExecute(argument))
        return false;

      command.Execute(argument);
      return true;
    }
  }
}