using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Example.Avalonia.Models;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Key;
using StudioLaValse.Geometry;
using System;
using StudioLaValse.Drawable.Interaction;

namespace StudioLaValse.Drawable.Example.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ModelFactory modelFactory;
    private readonly SceneFactory sceneFactory;
    private readonly SelectionWithKeyResponse<PersistentElement> selectionManager;
    private readonly INotifyEntityChanged<PersistentElement> notifyEntityChanged;
    private IDisposable? sceneManagerDispatcherDisposable;

    public CanvasViewModel CanvasViewModel { get; set; }

    public ICommand NewSceneCommand => ReactiveCommand.Create(
        () =>
        {
            var model = modelFactory.Create();
            var scene = sceneFactory.Create(model);
            var sceneManager = new InteractiveSceneManager<PersistentElement, int>(scene, e => e.ElementId.IntValue, CanvasViewModel.BaseBitmapPainter);

            sceneManager.Rerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = sceneManager;
            CanvasViewModel.Pipe = sceneManager.Then(selectionManager);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand ToggleZoom => ReactiveCommand.Create(
        () => 
        { 
            CanvasViewModel.EnablePan = !CanvasViewModel.EnablePan;
            CanvasViewModel.EnableZoom = !CanvasViewModel.EnableZoom;
        });

    public MainWindowViewModel(CanvasViewModel canvasViewModel, ModelFactory modelFactory, SceneFactory sceneFactory, SelectionWithKeyResponse<PersistentElement> selectionManager, INotifyEntityChanged<PersistentElement> notifyEntityChanged)
    {
        this.modelFactory = modelFactory;
        this.sceneFactory = sceneFactory;
        this.selectionManager = selectionManager;
        this.notifyEntityChanged = notifyEntityChanged;

        CanvasViewModel = canvasViewModel;

    }
}
