using StudioLaValse.Drawable.Example.WPF.Models;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.WPF.Commands;
using StudioLaValse.Drawable.WPF.ViewModels;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using System.Windows.Input;

namespace StudioLaValse.Drawable.Example.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ModelFactory modelFactory;
        private readonly SceneFactory sceneFactory;
        private readonly ISelectionManager<PersistentElement> selectionManager;
        private readonly INotifyEntityChanged<PersistentElement> notifyEntityChanged;
        private IDisposable? sceneManagerDispatcherDisposable;

        public CanvasViewModel CanvasViewModel { get; }

        public ICommand NewSceneCommand => new RelayCommand(
            () =>
            {
                var model = modelFactory.Create();
                var scene = sceneFactory.Create(model);
                var sceneManager = new SceneManager<PersistentElement, ElementId>(scene, e => e.ElementId, CanvasViewModel.BaseBitmapPainter).WithBackground(ColorARGB.Black).WithRerender();

                sceneManagerDispatcherDisposable?.Dispose();
                sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager);

                CanvasViewModel.Pipe = BehaviorPipeline.DoNothing()
                    .InterceptKeys(selectionManager, out var _selectionManager)
                    .ThenHandleDefaultMouseInteraction(sceneManager.VisualParents, notifyEntityChanged)
                    .ThenHandleMouseHover(sceneManager.VisualParents, notifyEntityChanged)
                    .ThenHandleDefaultClick(sceneManager.VisualParents, _selectionManager)
                    .ThenHandleSelectionBorder(sceneManager.VisualParents, _selectionManager, CanvasViewModel.SelectionBorder, notifyEntityChanged)
                    .ThenHandleTransformations(_selectionManager, sceneManager.VisualParents, notifyEntityChanged)
                    .ThenRender(notifyEntityChanged);

                CanvasViewModel.CenterContent(scene);
            },
            () =>
            {
                return true;
            });

        public ICommand ToggleZoom => new RelayCommand(
            () => 
            { 
                CanvasViewModel.EnablePan = !CanvasViewModel.EnablePan;
                CanvasViewModel.EnableZoom = CanvasViewModel.EnablePan;
            },
            () => true);


        public MainViewModel(CanvasViewModel canvasViewModel, ModelFactory modelFactory, SceneFactory sceneFactory, ISelectionManager<PersistentElement> selectionManager, INotifyEntityChanged<PersistentElement> notifyEntityChanged)
        {
            CanvasViewModel = canvasViewModel;
            this.modelFactory = modelFactory;
            this.sceneFactory = sceneFactory;
            this.selectionManager = selectionManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }
    }
}
