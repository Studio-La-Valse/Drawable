using Microsoft.Extensions.DependencyInjection;
using Example.WPF.ViewModels;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.ViewModels;
using StudioLaValse.Key;
using StudioLaValse.Drawable;

namespace Example.WPF.Models
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            services.AddTransient<IKeyGenerator<int>, IncrementalKeyGenerator>();

            var notifyElementChanged = SceneManager<ElementId>.CreateObservable();
            services.AddSingleton(SelectionManager<PersistentElement>.CreateDefault(e => e.ElementId).OnChangedNotify(notifyElementChanged, e => e.ElementId).InterceptKeys());
            services.AddSingleton<ISelectionManager<PersistentElement>>(s => s.GetRequiredService<SelectionWithKeyResponse<PersistentElement>>());
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
