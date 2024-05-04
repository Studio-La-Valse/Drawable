using Avalonia.ReactiveUI;
using StudioLaValse.Drawable.Example.Avalonia.ViewModels;

namespace StudioLaValse.Drawable.Example.Avalonia.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}