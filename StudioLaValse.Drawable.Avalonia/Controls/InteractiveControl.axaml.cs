using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using StudioLaValse.Drawable.Avalonia.Painters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Geometry;
using System.Drawing;

namespace StudioLaValse.Drawable.Avalonia.Controls;
/// <inheritdoc/>
public partial class InteractiveControl : BaseInteractiveControl, IDisposable
{
    private readonly GraphicsPainter baseBitmapPainter;
    private readonly DrawableElementObserver drawableElementObserver;


    private IObservable<BaseDrawableElement>? elementEmitter;
    private IDisposable? elementEmitterSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, IObservable<BaseDrawableElement>?> ElementEmitterProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, IObservable<BaseDrawableElement>?>(
             nameof(ElementEmitter),
             e => e.ElementEmitter,
             (e, v) => e.ElementEmitter = v);
    /// <summary>
    /// 
    /// </summary>
    public IObservable<BaseDrawableElement>? ElementEmitter
    {
        get => elementEmitter;
        set
        {
            elementEmitterSubscription?.Dispose();
            SetAndRaise(ElementEmitterProperty, ref elementEmitter, value);
            elementEmitterSubscription = value?.Subscribe(drawableElementObserver);
            baseBitmapPainter.FinishDrawing();
        }
    }

    private IPipe pipe;
    private IDisposable pipeSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, IPipe> PipeProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, IPipe>(
             nameof(Pipe),
             e => e.Pipe,
             (e, v) => e.Pipe = v);
    /// <summary>
    /// 
    /// </summary>
    public IPipe Pipe
    {
        get => pipe;
        set
        {
            pipeSubscription?.Dispose();
            SetAndRaise(PipeProperty, ref pipe, value);
            if(pipe is null)
            {
                return;
            }

            pipeSubscription = this.Subscribe(pipe);
        }
    }

    private ObservableBoundingBox? _selectionBorder;
    private IDisposable? selectionBorderSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, ObservableBoundingBox?> SelectionBorderProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, ObservableBoundingBox?>(
             nameof(SelectionBorder),
             e => e.SelectionBorder,
             (e, v) => e.SelectionBorder = v);
    /// <summary>
    /// 
    /// </summary>
    public ObservableBoundingBox? SelectionBorder
    {
        get => _selectionBorder;
        set
        {
            selectionBorderSubscription?.Dispose();
            SetAndRaise(SelectionBorderProperty, ref _selectionBorder, value);
            selectionBorderSubscription = value?.Subscribe(this.selectionBorderName.CreateObserver(this));
        }
    }

    private bool enablePan;
    private IDisposable? enablePanSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, bool> EnablePanProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, bool>(
             nameof(EnablePan),
             e => e.EnablePan,
             (e, v) => e.EnablePan = v);
    /// <summary>
    /// 
    /// </summary>
    public bool EnablePan
    {
        get => enablePan;
        set
        {
            enablePanSubscription?.Dispose();
            SetAndRaise(EnablePanProperty, ref enablePan, value);
            if (enablePan)
            {
                enablePanSubscription = this.EnablePan();
            }
        }
    }


    private bool enableZoom;
    private IDisposable? enableZoomSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, bool> EnableZoomProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, bool>(
             nameof(EnableZoom),
             e => e.EnableZoom,
             (e, v) => e.EnableZoom = v);
    /// <summary>
    /// 
    /// </summary>
    public bool EnableZoom
    {
        get => enableZoom;
        set
        {
            enableZoomSubscription?.Dispose();
            SetAndRaise(EnableZoomProperty, ref enableZoom, value);
            if (enableZoom)
            {
                enableZoomSubscription = this.EnableZoom();
            }
        }
    }




    /// <summary>
    /// 
    /// </summary>
    public InteractiveControl()
    {
        InitializeComponent();

        var textMeasurer = new AvaloniaTextMeasurer();
        ExternalTextMeasure.TextMeasurer = textMeasurer;

        baseBitmapPainter = new GraphicsPainter(this, textMeasurer);
        drawableElementObserver = new DrawableElementObserver(baseBitmapPainter);

        pipe = Pipeline.DoNothing();
        pipeSubscription = this.Subscribe(pipe);
    }

    class DrawableElementObserver : IObserver<BaseDrawableElement>
    {
        private readonly GraphicsPainter baseBitmapPainter;

        private bool completed = true;

        public DrawableElementObserver(GraphicsPainter baseBitmapPainter)
        {
            this.baseBitmapPainter = baseBitmapPainter;
        }

        public void OnCompleted()
        {
            baseBitmapPainter.FinishDrawing();

            completed = true;
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(BaseDrawableElement value)
        {
            if (completed)
            {
                baseBitmapPainter.InitDrawing();
                completed = false;
            }

            baseBitmapPainter.DrawElement(value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        enableZoomSubscription?.Dispose();
        enablePanSubscription?.Dispose();
        selectionBorderSubscription?.Dispose();
        pipeSubscription.Dispose();
        elementEmitterSubscription?.Dispose();
    }
}
