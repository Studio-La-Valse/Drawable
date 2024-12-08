using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using StudioLaValse.Drawable.Avalonia.Painters;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Interaction;
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

    private IInputObserver inputObserver;
    private IDisposable inputObserverSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, IInputObserver> InputObserverProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, IInputObserver>(
             nameof(InputObserver),
             e => e.InputObserver,
             (e, v) => e.InputObserver = v);
    /// <summary>
    /// 
    /// </summary>
    public IInputObserver InputObserver
    {
        get => InputObserver;
        set
        {
            inputObserverSubscription?.Dispose();
            SetAndRaise(InputObserverProperty, ref inputObserver, value);
            if(inputObserver is null)
            {
                return;
            }

            inputObserverSubscription = this.Subscribe(inputObserver);
        }
    }

    private IObservable<BoundingBox>? _selectionBorder;
    private IDisposable? selectionBorderSubscription;
    /// <summary>
    /// 
    /// </summary>
    public static readonly DirectProperty<InteractiveControl, IObservable<BoundingBox>?> SelectionBorderProperty =
         AvaloniaProperty.RegisterDirect<InteractiveControl, IObservable<BoundingBox>?>(
             nameof(SelectionBorder),
             e => e.SelectionBorder,
             (e, v) => e.SelectionBorder = v);
    /// <summary>
    /// 
    /// </summary>
    public IObservable<BoundingBox>? SelectionBorder
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

        inputObserver = new BaseInputObserver();
        inputObserverSubscription = this.Subscribe(inputObserver);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        enableZoomSubscription?.Dispose();
        enablePanSubscription?.Dispose();
        selectionBorderSubscription?.Dispose();
        inputObserverSubscription.Dispose();
        elementEmitterSubscription?.Dispose();
    }
}
