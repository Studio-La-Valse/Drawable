using Microsoft.Extensions.DependencyInjection;
using StudioLaValse.Drawable.Example.WPF.ViewModels;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.WPF.Models
{
    public static class ServiceCollectionExtensions
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
            services.AddSingleton<MainViewModel>();
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
