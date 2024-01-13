using StudioLaValse.Drawable.WPF.Interfaces;
using System.Windows.Input;

namespace StudioLaValse.Drawable.WPF.Commands
{
    public class CommandFactory : ICommandFactory
    {
        public ICommand Create(Action action, Func<bool>? canExecute = null)
        {
            return new RelayCommand(action, canExecute);
        }

        public ICommand Create<T>(Action<T> action, Func<T, bool>? canExecute = null)
        {
            return new RelayCommand<T>(action, canExecute);
        }
    }
}
