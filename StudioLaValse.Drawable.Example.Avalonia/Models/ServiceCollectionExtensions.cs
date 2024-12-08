using Microsoft.Extensions.DependencyInjection;
using StudioLaValse.Drawable.Example.Avalonia.ViewModels;
using StudioLaValse.Drawable.Example.Avalonia.Views;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.ViewModels;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Avalonia.Models
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            services.AddTransient<IKeyGenerator<int>, IncrementalKeyGenerator>();

            var notifyElementChanged = SceneManager<ElementId>.CreateObservable();
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
            services.AddSingleton<MainWindow>();
            return services;
        }
    }

}
