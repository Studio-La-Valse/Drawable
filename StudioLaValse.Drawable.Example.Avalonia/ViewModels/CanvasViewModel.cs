using ReactiveUI;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Key;
using System;
using System.Reactive.Linq;

namespace StudioLaValse.Drawable.Example.Avalonia.ViewModels;

public class CanvasViewModel : ViewModelBase
{
    private ObservableBoundingBox? selectionBorder;
    private IObservable<PersistentElement>? invalidator;
    private SceneManager<PersistentElement, ElementId>? sceneManager;
    private bool enablePan;
    private IPipe? pipe;

    public ObservableBoundingBox? SelectionBorder { get => selectionBorder; set => this.RaiseAndSetIfChanged(ref selectionBorder, value); }

    public IObservable<PersistentElement>? Invalidator { get => invalidator; set => this.RaiseAndSetIfChanged(ref invalidator, value); }

    public SceneManager<PersistentElement, ElementId>? SceneManager { get => sceneManager; set => this.RaiseAndSetIfChanged(ref sceneManager, value); }

    public bool EnablePan { get => enablePan; set => this.RaiseAndSetIfChanged(ref enablePan, value); }

    public IPipe? Pipe { get => pipe; set => this.RaiseAndSetIfChanged(ref pipe, value); } 

    public CanvasViewModel(INotifyEntityChanged<PersistentElement> observable)
    {
        Invalidator = observable;
        EnablePan = true;
        SelectionBorder = new ObservableBoundingBox();
    }
}
