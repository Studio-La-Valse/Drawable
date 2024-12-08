using StudioLaValse.Drawable.Example.WPF.Models;
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

namespace StudioLaValse.Drawable.Example.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ModelFactory modelFactory;
        private readonly SceneFactory sceneFactory;
        private readonly SelectionWithKeyResponse<PersistentElement> selectionManager;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
        private IDisposable? sceneManagerDispatcherDisposable;

        public CanvasViewModel CanvasViewModel { get; }

        public ICommand NewSceneCommand => new RelayCommand(
            () =>
            {
                var model = modelFactory.Create();
                var scene = sceneFactory.Create(model);
                var sceneManager = new InteractiveSceneManager<ElementId>(scene, CanvasViewModel.CanvasPainter);

                sceneManager.Rerender();

                sceneManagerDispatcherDisposable?.Dispose();
                sceneManagerDispatcherDisposable = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

                CanvasViewModel.SelectionBorder = sceneManager;
                CanvasViewModel.InputObserver = new BaseInputObserver().Then(sceneManager).Then(selectionManager);

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


        public MainViewModel(CanvasViewModel canvasViewModel, ModelFactory modelFactory, SceneFactory sceneFactory, SelectionWithKeyResponse<PersistentElement> selectionManager, INotifyEntityChanged<ElementId> notifyEntityChanged)
        {
            CanvasViewModel = canvasViewModel;
            this.modelFactory = modelFactory;
            this.sceneFactory = sceneFactory;
            this.selectionManager = selectionManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }
    }
}
