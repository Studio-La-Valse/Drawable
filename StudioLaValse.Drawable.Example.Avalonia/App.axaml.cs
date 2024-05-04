using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.Avalonia.Painters;
using StudioLaValse.Drawable.Example.Avalonia.ViewModels;
using StudioLaValse.Drawable.Example.Avalonia.Views;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.Linq;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StudioLaValse.Drawable.Example.Avalonia.Models;
using MsExtensionsHostingSample;
using StudioLaValse.Drawable.Example.Avalonia.Controls;

namespace StudioLaValse.Drawable.Example.Avalonia;
public partial class App : Application
{
    public IHost? GlobalHost { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var hostBuilder = CreateHostBuilder();
        var host = hostBuilder.Build();
        GlobalHost = host;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = GlobalHost.Services.GetRequiredService<MainWindowViewModel>()
            };
            desktop.Exit += (sender, args) =>
            {
                GlobalHost.StopAsync(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                GlobalHost.Dispose();
                GlobalHost = null;
            };
        }

        DataTemplates.Add(GlobalHost.Services.GetRequiredService<ViewLocator>());

        base.OnFrameworkInitializationCompleted();

        // Usually, we don't want to block main UI thread.
        // But if it's required to start async services before we create any window,
        // then don't set any MainWindow, and simply call Show() on a new window later after async initialization. 
        await host.StartAsync();
    }

    private static HostApplicationBuilder CreateHostBuilder()
    {
        // Alternatively, we can use Host.CreateDefaultBuilder, but this sample focuses on HostApplicationBuilder.
        var builder = Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());
        builder.Services.AddModels().AddViewModels().AddViews();
        return builder;
    }


}

file static class Extensions
{
    public static IServiceCollection AddModels(this IServiceCollection services)
    {
        services.AddTransient<IKeyGenerator<int>, IncrementalKeyGenerator>();

        var notifyElementChanged = SceneManager<PersistentElement, ElementId>.CreateObservable();
        services.AddSingleton(SelectionManager<PersistentElement>.CreateDefault(e => e.ElementId).OnChangedNotify(notifyElementChanged, e => e.ElementId));
        services.AddSingleton(notifyElementChanged);
        services.AddTransient<ModelFactory>();
        services.AddTransient<SceneFactory>();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<CanvasViewModel>();
        return services;
    }

    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ViewLocator>();
        services.AddView<CanvasViewModel, MainControl>();
        return services;
    }
}

public static class DesignData
{
    public static MainWindowViewModel MainWindowViewModel { get; } =
        ((App)Application.Current!).GlobalHost!.Services.GetRequiredService<MainWindowViewModel>();
}