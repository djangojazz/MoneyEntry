using System;
using System.Windows.Input;

namespace MoneyEntry.ViewModel
{
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(string displayName, ICommand command)
        {
          base.DisplayName = displayName;
          Command = command ?? throw new ArgumentNullException("command");
        }

        public ICommand Command { get; private set; }
    }
}