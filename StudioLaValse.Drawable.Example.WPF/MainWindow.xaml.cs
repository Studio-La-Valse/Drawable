using StudioLaValse.Drawable.Example.WPF.ViewModels;
using System.Windows;

namespace StudioLaValse.Drawable.Example.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;
        }
    }
}
