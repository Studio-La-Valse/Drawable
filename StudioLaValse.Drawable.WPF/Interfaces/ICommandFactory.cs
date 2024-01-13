using System.Windows.Input;

namespace StudioLaValse.Drawable.WPF.Interfaces
{
    public interface ICommandFactory
    {
        public ICommand Create(Action action, Func<bool>? canExecute = null);
        public ICommand Create<T>(Action<T> action, Func<T, bool>? canExecute = null);
    }
}
