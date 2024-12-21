using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.DependencyProperties;
using StudioLaValse.Drawable.WPF.Painters;
using StudioLaValse.Drawable.WPF.Text;
using StudioLaValse.Drawable.WPF.Visuals;
using StudioLaValse.Geometry;
using System;
using System.Windows;
using System.Windows.Media;

namespace StudioLaValse.Drawable.WPF.UserControls
{
    public partial class WindowsDrawingContextUserControl : BaseInteractiveVisual
    {
        private readonly WindowsDrawingContextBitmapPainter baseBitmapPainter;
        private readonly DrawableElementObserver drawableElementObserver;


        private IDisposable? elementEmitterSubscription;
        public static readonly DependencyProperty ElementEmitterProperty = DependencyPropertyBase
            .Register<WindowsDrawingContextUserControl, IObservable<BaseDrawableElement>>(nameof(ElementEmitter), (o, e) =>
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




        private IDisposable inputObserverSubscription;
        public static readonly DependencyProperty InputObserverProperty = DependencyPropertyBase
            .Register<WindowsDrawingContextUserControl, IInputObserver?>(nameof(InputObserver), (o, e) =>
            {
                o.inputObserverSubscription?.Dispose();
                if (e is null)
                {
                    return;
                }

                o.inputObserverSubscription = o.Subscribe(e);
            }, new BaseInputObserver());
        public IInputObserver? InputObserver
        {
            get => (IInputObserver?)GetValue(InputObserverProperty);
            set => SetValue(InputObserverProperty, value);
        }




        private IDisposable? selectionBorderSubscription;
        public static readonly DependencyProperty SelectionBorderProperty = DependencyPropertyBase
            .Register<WindowsDrawingContextUserControl, IObservable<BoundingBox>?>(nameof(SelectionBorder), (o, e) =>
            {
                o.selectionBorderSubscription?.Dispose();
                o.selectionBorderSubscription = e?.Subscribe(o.selectionBorderName.CreateObserver(o));
            });
        public IObservable<BoundingBox>? SelectionBorder
        {
            get => (IObservable<BoundingBox>)GetValue(SelectionBorderProperty);
            set => SetValue(SelectionBorderProperty, value);
        }




        private IDisposable? enablePanSubscription;
        public static readonly DependencyProperty EnablePanProperty = DependencyPropertyBase
            .Register<WindowsDrawingContextUserControl, bool>(nameof(EnablePan), (o, e) =>
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
            .Register<WindowsDrawingContextUserControl, bool>(nameof(EnableZoom), (o, e) =>
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



        public List<Action<DrawingContext>> Cache = [];



        public WindowsDrawingContextUserControl()
        {
            InitializeComponent();

            var textMeasurer = new WPFTextMeasurer();
            baseBitmapPainter = new WindowsDrawingContextBitmapPainter(this, textMeasurer);
            drawableElementObserver = new DrawableElementObserver(baseBitmapPainter);

            InputObserver = new BaseInputObserver();
            inputObserverSubscription = this.Subscribe(InputObserver);    
        }

        public override void Refresh()
        {
            using var drawingContext = this.drawingVisual.RenderOpen();
            drawingContext.PushTransform(new ScaleTransform(Zoom, Zoom));
            drawingContext.PushTransform(new TranslateTransform(TranslateX, TranslateY));

            foreach (var action in Cache)
            {
                action(drawingContext);
            }

            drawingContext.Close();
        }

        private void BaseInteractiveVisual_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.Focus();
        }  
    }
}
