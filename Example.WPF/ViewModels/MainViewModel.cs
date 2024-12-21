using Example.Model;
using Example.Scene;
using Example.WPF.Models;
using StudioLaValse.Drawable;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Interaction.ViewModels;
using StudioLaValse.Drawable.WPF.Commands;
using StudioLaValse.Drawable.WPF.ViewModels;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using System.Windows.Input;

namespace Example.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly SelectionWithKeyResponse<PersistentElement> selectionManager;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
        private IDisposable? sceneManagerDispatcherDisposable;

        public CanvasViewModel CanvasViewModel { get; }

        public static IInputObserver CreateDefaultObserver(SelectionWithKeyResponse<PersistentElement> selectionManager,
        SceneManager<ElementId> sceneManager,
        SelectionBorder selectionBorder,
        INotifyEntityChanged<ElementId> notifyEntityChanged)
    {
        return new BaseInputObserver()
            .NotifySelection(selectionManager)
            .AddSelectionBorder(sceneManager, selectionBorder)
            .AddMassTransformations(sceneManager)
            .AddDefaultBehavior(sceneManager)
            .AddRerender(notifyEntityChanged);
    }

    public ICommand PointsSceneCommand => new RelayCommand(
        () =>
        {
            var model = new PointsModel(keyGenerator, notifyEntityChanged);
            var scene = new VisualPoints(model, selectionManager, notifyEntityChanged);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new SceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).WithRerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = CreateDefaultObserver(selectionManager, sceneManager, selectionBorder, notifyEntityChanged);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand CurveSceneCommand => new RelayCommand(
        () =>
        {
            var model = new CurveModel(keyGenerator, notifyEntityChanged);
            var scene = new VisualCurve(model, selectionManager, notifyEntityChanged);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new SceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).WithRerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = CreateDefaultObserver(selectionManager, sceneManager, selectionBorder, notifyEntityChanged);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand GraphSceneCommand => new RelayCommand(
        () =>
        {
            var model = new Graph(keyGenerator);
            var scene = new VisualGraph(model, selectionManager, notifyEntityChanged);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new SceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).WithRerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = CreateDefaultObserver(selectionManager, sceneManager, selectionBorder, notifyEntityChanged);

            CanvasViewModel.CenterContent(scene);
        });

    public ICommand TextSceneCommand => new RelayCommand(
        () =>
        {
            var model = new TextModel(keyGenerator);
            var scene = new TextScene(model);
            var selectionBorder = new SelectionBorder();
            var sceneManager = new SceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter).WithRerender();

            sceneManagerDispatcherDisposable?.Dispose();
            sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

            CanvasViewModel.SelectionBorder = selectionBorder;
            CanvasViewModel.InputObserver = CreateDefaultObserver(selectionManager, sceneManager, selectionBorder, notifyEntityChanged);

            CanvasViewModel.CenterContent(scene);
        });


        public MainViewModel(CanvasViewModel canvasViewModel, IKeyGenerator<int> keyGenerator, SelectionWithKeyResponse<PersistentElement> selectionManager, INotifyEntityChanged<ElementId> notifyEntityChanged)
        {
            CanvasViewModel = canvasViewModel;
            this.keyGenerator = keyGenerator;
            this.selectionManager = selectionManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }
    }
}
