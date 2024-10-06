using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.DependencyProperties;
using StudioLaValse.Drawable.WPF.Painters;
using StudioLaValse.Drawable.WPF.Text;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudioLaValse.Drawable.Example.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for CanvasUserControl.xaml
    /// </summary>
    public partial class CanvasUserControl : UserControl, IObserver<InvalidationRequest<PersistentElement>>
    {
        public static readonly DependencyProperty SceneProperty = DependencyPropertyBase
            .Register<CanvasUserControl, SceneManager<PersistentElement, ElementId>>(nameof(SceneManager), (o, e) => { e.Rerender(o.BaseBitmapPainter); });

        public static readonly DependencyProperty SelectionBorderProperty = DependencyPropertyBase
            .Register<CanvasUserControl, IObservable<BoundingBox>>(nameof(SelectionBorder), (canvas, value) =>
            {
                canvas.SelectionBorderDisposable?.Dispose();
                canvas.SelectionBorderDisposable = value.Subscribe(canvas.selectionBorder.CreateObserver(canvas.InteractiveCanvas));
            });

        public static readonly DependencyProperty EnablePanProperty = DependencyPropertyBase
            .Register<CanvasUserControl, bool>(nameof(EnablePan), (canvas, pan) =>
            {
                canvas.PanEnabledDisposable?.Dispose();
                canvas.ZoomEnabledDisposable?.Dispose();

                if (pan)
                {
                    canvas.PanEnabledDisposable = canvas.InteractiveCanvas.EnablePan();
                    canvas.ZoomEnabledDisposable = canvas.InteractiveCanvas.EnableZoom();
                }

            }, true);

        public static readonly DependencyProperty InvalidatorProperty = DependencyPropertyBase
            .Register<CanvasUserControl, INotifyEntityChanged<PersistentElement>>(nameof(Invalidator), (canvas, newval) =>
            {
                canvas.InvalidatorDisposable?.Dispose();
                canvas.InvalidatorDisposable = newval.Subscribe(canvas);
            });

        public static readonly DependencyProperty PipeProperty = DependencyPropertyBase
            .Register<CanvasUserControl, IPipe>(nameof(Pipe), (canvas, newval) =>
            {
                canvas.PipeDisposable?.Dispose();
                canvas.PipeDisposable = canvas.InteractiveCanvas.Subscribe(newval);
            });



        public SceneManager<PersistentElement, ElementId>? SceneManager
        {
            get => (SceneManager<PersistentElement, ElementId>)GetValue(SceneProperty);
            set => SetValue(SceneProperty, value);
        }
        public IObservable<BoundingBox>? SelectionBorder
        {
            get => (IObservable<BoundingBox>)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }
        public bool EnablePan
        {
            get => (bool)GetValue(EnablePanProperty);
            set => SetValue(EnablePanProperty, value);
        }
        public INotifyEntityChanged<PersistentElement> Invalidator
        {
            get => (INotifyEntityChanged<PersistentElement>)GetValue(InvalidatorProperty);
            set => SetValue(InvalidatorProperty, value);
        }
        public IPipe Pipe
        {
            get => (IPipe)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }



        public IDisposable? PanEnabledDisposable { get; set; }
        public IDisposable? ZoomEnabledDisposable { get; set; }
        public IDisposable? InvalidatorDisposable { get; set; }
        public IDisposable? SelectionBorderDisposable { get; set; }
        public IDisposable? PipeDisposable { get; set; }
        public BaseBitmapPainter BaseBitmapPainter { get; }
        public IInteractiveCanvas InteractiveCanvas => canvas;



        public CanvasUserControl()
        {
            InitializeComponent();

            var textMeasurer = new WPFTextMeasurer();
            ExternalTextMeasure.TextMeasurer = textMeasurer;
            BaseBitmapPainter = new WindowsDrawingContextBitmapPainter(canvas, textMeasurer);

            PanEnabledDisposable = canvas.EnablePan();
            ZoomEnabledDisposable = canvas.EnableZoom();
        }



        public void OnCompleted()
        {
            if (SceneManager is null)
            {
                throw new Exception("No scenemanager active to invalidate this element.");
            }

            SceneManager.RenderChanges(BaseBitmapPainter);
        }
        public void OnError(Exception error)
        {
            throw error;
        }
        public void OnNext(InvalidationRequest<PersistentElement> value)
        {
            if (SceneManager is null)
            {
                throw new Exception("No scenemanager active to invalidate this element.");
            }

            SceneManager.AddToQueue(value);
        }
    }
}
