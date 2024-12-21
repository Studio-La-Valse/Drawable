using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Key;
using System;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.ViewModels;
using Example.Model;
using System.Linq;
using Example.Scene;
using StudioLaValse.Drawable;
using StudioLaValse.Drawable.Avalonia.Painters;
using StudioLaValse.Drawable.Text;

namespace Example.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly SelectionWithKeyResponse<PersistentElement> selectionManager;
    private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
    private readonly IKeyGenerator<int> keyGenerator;
    private IDisposable? sceneManagerDispatcherDisposable;

    public CanvasViewModel CanvasViewModel { get; set; }

    public ICommand PointsSceneCommand => ReactiveCommand.Create(
        () =>
        {
            var model = new PointsModel(keyGenerator, notifyEntityChanged);
            var scene = new VisualPoints(model, selectionManager, notifyEntityChanged);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new InteractiveSceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).AddSelectionBorder(selectionBorder);

            sceneManager.Rerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = new BaseInputObserver().Then(sceneManager).Then(selectionManager);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand CurveSceneCommand => ReactiveCommand.Create(
        () =>
        {
            var model = new CurveModel(keyGenerator, notifyEntityChanged);
            var scene = new VisualCurve(model, selectionManager, notifyEntityChanged);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new InteractiveSceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).AddSelectionBorder(selectionBorder);

            sceneManager.Rerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = new BaseInputObserver().Then(sceneManager).Then(selectionManager);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand GraphSceneCommand => ReactiveCommand.Create(
        () =>
        {
            var model = new Graph(keyGenerator);
            var scene = new VisualGraph(model, selectionManager, notifyEntityChanged);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new InteractiveSceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).AddSelectionBorder(selectionBorder);

            sceneManager.Rerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = new BaseInputObserver().Then(sceneManager).Then(selectionManager);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand TextSceneCommand => ReactiveCommand.Create(
        () =>
        {
            var model = new TextModel(keyGenerator);
            var scene = new TextScene(model);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new InteractiveSceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).AddSelectionBorder(selectionBorder);

            sceneManager.Rerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = new BaseInputObserver().Then(sceneManager).Then(selectionManager);

            CanvasViewModel.CenterContent(scene);
        });

    public MainWindowViewModel(CanvasViewModel canvasViewModel, SelectionWithKeyResponse<PersistentElement> selectionManager, INotifyEntityChanged<ElementId> notifyEntityChanged, IKeyGenerator<int> keyGenerator)
    {
        this.selectionManager = selectionManager;
        this.notifyEntityChanged = notifyEntityChanged;
        this.keyGenerator = keyGenerator;

        CanvasViewModel = canvasViewModel;
    }
}
