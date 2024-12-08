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
using StudioLaValse.Key;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StudioLaValse.Drawable.Example.Avalonia.Models;
using MsExtensionsHostingSample;
using System.Diagnostics;

namespace StudioLaValse.Drawable.Example.Avalonia;
public partial class App : Application
{
    public IHost? GlobalHost { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        using var host = CreateHostBuilder().Build();
        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        var mainViewModel = host.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.DataContext = mainViewModel;

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            throw new UnreachableException();
        }

        desktop.MainWindow = mainWindow;

        base.OnFrameworkInitializationCompleted();
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
        services.AddSingleton(SelectionManager<PersistentElement>.CreateDefault(e => e.ElementId).OnChangedNotify(notifyElementChanged, e => e.ElementId).InterceptKeys());
        services.AddSingleton<ISelectionManager<PersistentElement>>(s => s.GetRequiredService<SelectionWithKeyResponse<PersistentElement>>());
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
        services.AddTransient<MainWindow>();
        return services;
    }
}
