using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.WPF.ViewModels;
using StudioLaValse.Key;
using System;

namespace StudioLaValse.Drawable.Example.WPF.ViewModels
{
    public class CanvasViewModel : BaseViewModel
    {
        public ObservableBoundingBox SelectionBorder
        {
            get => GetValue(() => SelectionBorder);
            set => SetValue(() => SelectionBorder, value);
        }

        public IObservable<PersistentElement>? Invalidator
        {
            get => GetValue(() => Invalidator);
            set => SetValue(() => Invalidator, value);
        }

        public SceneManager<PersistentElement>? Scene
        {
            get => GetValue(() => Scene);
            set => SetValue(() => Scene, value);
        }

        public bool EnablePan
        {
            get => GetValue(() => EnablePan);
            set => SetValue(() => EnablePan, value);
        }

        public IPipe Pipe
        {
            get => GetValue(() => Pipe);
            set => SetValue(() => Pipe, value);
        }

        public CanvasViewModel(INotifyEntityChanged<PersistentElement> observable)
        {
            Invalidator = observable;
            EnablePan = true;
            SelectionBorder = new ObservableBoundingBox();
        }
    }
}
