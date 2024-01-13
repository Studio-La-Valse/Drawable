using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudioLaValse.Drawable.Example.WPF.Models;
using System.Windows;

namespace StudioLaValse.Drawable.Example.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = Host.CreateDefaultBuilder()
               .ConfigureServices(services =>
               {
                   services
                       .AddModels()
                       .AddViewModels()
                       .AddViews();
               });
            var host = builder.Build();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
