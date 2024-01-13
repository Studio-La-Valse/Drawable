using System.Windows.Input;

namespace StudioLaValse.Drawable.WPF.ViewModels
{
    public class MenuItemViewModel
    {
        public List<MenuItemViewModel?> MenuItems { get; set; }


        public string Header { get; }
        public object? CommandParameter { get; }
        public ICommand? Command { get; }


        public MenuItemViewModel(string header, ICommand? command = null, object? commandParameter = null)
        {
            Command = command;
            MenuItems = new List<MenuItemViewModel?>();
            Header = header;
            CommandParameter = commandParameter;
        }
    }
}
