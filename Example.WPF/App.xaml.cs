using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Example.WPF.Models;
using System.Windows;
using System.Windows.Media;

namespace Example.WPF
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
