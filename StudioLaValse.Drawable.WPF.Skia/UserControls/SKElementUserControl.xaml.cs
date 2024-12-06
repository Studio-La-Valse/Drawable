using SkiaSharp;
using SkiaSharp.Views.Desktop;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Skia.Models;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.DependencyProperties;
using StudioLaValse.Drawable.WPF.Painters;
using StudioLaValse.Drawable.WPF.Skia.BitmapPainters;
using StudioLaValse.Drawable.WPF.UserControls;
using StudioLaValse.Drawable.WPF.Visuals;
using System.Windows;
using StudioLaValse.Drawable.Interaction;

namespace StudioLaValse.Drawable.WPF.Skia.UserControls
{
    /// <summary>
    /// The default Skia implementation for WPF purposes.
    /// </summary>
    public partial class SKElementUserControl : BaseInteractiveVisual
    {
        private readonly SkiaWpfElementPainter baseBitmapPainter;
        private readonly DrawableElementObserver drawableElementObserver;


        private IDisposable? elementEmitterSubscription;
        public static readonly DependencyProperty ElementEmitterProperty = DependencyPropertyBase
            .Register<SKElementUserControl, IObservable<BaseDrawableElement>>(nameof(ElementEmitter), (o, e) =>
            {
                o.elementEmitterSubscription?.Dispose();
                o.elementEmitterSubscription = e?.Subscribe(o.drawableElementObserver);
                o.baseBitmapPainter.FinishDrawing();

            });
        public IObservable<BaseDrawableElement> ElementEmitter
        {
            get => (IObservable<BaseDrawableElement>)GetValue(ElementEmitterProperty);
            set => SetValue(ElementEmitterProperty, value);
        }




        private IDisposable pipeSubscription;
        public static readonly DependencyProperty PipeProperty = DependencyPropertyBase
            .Register<SKElementUserControl, IInputObserver>(nameof(Pipe), (o, e) =>
            {
                o.pipeSubscription?.Dispose();
                if (e is null)
                {
                    return;
                }

                o.pipeSubscription = o.Subscribe(e);
            }, BehaviorPipeline.DoNothing());
        public IInputObserver Pipe
        {
            get => (IInputObserver)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }




        private IDisposable? selectionBorderSubscription;
        public static readonly DependencyProperty SelectionBorderProperty = DependencyPropertyBase
            .Register<SKElementUserControl, ObservableBoundingBox>(nameof(SelectionBorder), (o, e) =>
            {
                o.selectionBorderSubscription?.Dispose();
                o.selectionBorderSubscription = e?.Subscribe(o.selectionBorderName.CreateObserver(o));
            });
        public ObservableBoundingBox? SelectionBorder
        {
            get => (ObservableBoundingBox)GetValue(SelectionBorderProperty);
            set => SetValue(SelectionBorderProperty, value);
        }




        private IDisposable? enablePanSubscription;
        public static readonly DependencyProperty EnablePanProperty = DependencyPropertyBase
            .Register<SKElementUserControl, bool>(nameof(EnablePan), (o, e) =>
            {
                o.enablePanSubscription?.Dispose();
                if (e)
                {
                    o.enablePanSubscription = o.EnablePan();
                }
            }, false);
        public bool EnablePan
        {
            get => (bool)GetValue(EnablePanProperty);
            set => SetValue(EnablePanProperty, value);
        }




        private IDisposable? enableZoomSubscription;
        public static readonly DependencyProperty EnableZoomProperty = DependencyPropertyBase
            .Register<SKElementUserControl, bool>(nameof(EnableZoom), (o, e) =>
            {
                o.enableZoomSubscription?.Dispose();
                if (e)
                {
                    o.enableZoomSubscription = o.EnableZoom();
                }

            });
        public bool EnableZoom
        {
            get => (bool)GetValue(EnableZoomProperty);
            set => SetValue(EnableZoomProperty, value);
        }



        public List<Action<SKCanvas>> Cache = [];

        public SKElementUserControl()
        {
            InitializeComponent();

            this.SKElement.IgnorePixelScaling = true;

            var textMeasurer = new SkiaTextMeasurer();
            ExternalTextMeasure.TextMeasurer = textMeasurer;

            baseBitmapPainter = new SkiaWpfElementPainter(this, textMeasurer);
            drawableElementObserver = new DrawableElementObserver(baseBitmapPainter);

            Pipe = BehaviorPipeline.DoNothing();
            pipeSubscription = this.Subscribe(Pipe);
        }


        public override void Refresh() => this.SKElement.InvalidateVisual();

        public void SKElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var graphics = e.Surface.Canvas;
            graphics.Clear();
            graphics.Scale((float)Zoom, (float)Zoom);
            graphics.Translate((float)TranslateX, (float)TranslateY);

            foreach (var action in Cache)
            {
                action(graphics);
            }
        }

        private void BaseInteractiveVisual_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Focus();
        }
    }
}
