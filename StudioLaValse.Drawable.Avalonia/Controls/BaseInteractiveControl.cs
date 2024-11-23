using Avalonia.Controls;
using Avalonia.Input;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Reactive.Linq;
using Avalonia.Media;
using System.Diagnostics;
using Avalonia;

namespace StudioLaValse.Drawable.Avalonia.Controls;

/// <inheritdoc/>
public class BaseInteractiveControl : UserControl, IInteractiveCanvas
{
    private readonly ButtonObservable leftButtonObservable = new ButtonObservable();
    private readonly ButtonObservable rightButtonObservable = new ButtonObservable();

    private bool leftButtonIsDown;
    private bool rightButtonIsDown;


    /// <inheritdoc/>
    public static readonly DirectProperty<BaseInteractiveControl, double> ZoomProperty = AvaloniaProperty
        .RegisterDirect<BaseInteractiveControl, double>(nameof(Zoom), o => o.Zoom, (o, v) => o.Zoom = v, 1);
    /// <inheritdoc/>
    public static readonly DirectProperty<BaseInteractiveControl, double> TranslateXProperty = AvaloniaProperty
        .RegisterDirect<BaseInteractiveControl, double>(nameof(TranslateX), o => o.TranslateX, (o, v) => o.TranslateX = v, 0);
    /// <inheritdoc/>
    public static readonly DirectProperty<BaseInteractiveControl, double> TranslateYProperty = AvaloniaProperty
        .RegisterDirect<BaseInteractiveControl, double>(nameof(TranslateY), o => o.TranslateY, (o, v) => o.TranslateY = v, 0);

    /// <inheritdoc/>
    public List<Action<DrawingContext>> DrawActions = [];


    private double zoom = 1;
    /// <inheritdoc/>
    public double Zoom
    {
        get => zoom;
        set
        {
            SetAndRaise(ZoomProperty, ref zoom, value);
            InvalidateVisual();
        }
    }

    private double translateX;
    /// <inheritdoc/>
    public double TranslateX
    {
        get => translateX;
        set
        {
            SetAndRaise(TranslateXProperty, ref translateX, value);
            InvalidateVisual();
        }
    }

    private double translateY;
    /// <inheritdoc/>
    public double TranslateY
    {
        get => translateY;
        set
        {
            SetAndRaise(TranslateYProperty, ref translateY, value);
            InvalidateVisual();
        }
    }

    /// <inheritdoc/>
    public IObservable<XY> MouseMove { get; }
    /// <inheritdoc/>
    public IObservable<bool> MouseLeftButtonDown { get; }
    /// <inheritdoc/>
    public IObservable<bool> MouseRightButtonDown { get; }
    /// <inheritdoc/>
    public IObservable<double> MouseWheel { get; }
    /// <inheritdoc/>
    new public IObservable<Interaction.UserInput.Key> KeyDown { get; }
    /// <inheritdoc/>
    new public IObservable<Interaction.UserInput.Key> KeyUp { get; }



    /// <inheritdoc/>
    public BaseInteractiveControl()
    {
        Focusable = true;
        IsHitTestVisible = true;

        XY getFromArgs(PointerEventArgs args)
        {
            var e = args.GetPosition(this);
            var pointOnCanvas = this.HostToCanvas(new XY(e.X, e.Y));
            return pointOnCanvas;
        }

        PointerPressed += BaseInteractiveControl_PointerPressed;
        PointerReleased += BaseInteractiveControl_PointerReleased;

        MouseMove = Observable.FromEventPattern<EventHandler<PointerEventArgs>, PointerEventArgs>(x => base.PointerMoved += x, x => base.PointerMoved -= x)
            .Select(e => e.EventArgs)
            .Select(getFromArgs);

        MouseLeftButtonDown = leftButtonObservable;

        MouseRightButtonDown = rightButtonObservable;

        MouseWheel = Observable.FromEventPattern<EventHandler<PointerWheelEventArgs>, PointerWheelEventArgs>(x => base.PointerWheelChanged += x, x => PointerWheelChanged -= x)
           .Select(e => e.EventArgs)
           .Select(e => e.Delta.Y / 10d);

        KeyDown = Observable.FromEventPattern<EventHandler<KeyEventArgs>, KeyEventArgs>(x => base.KeyDown += x, x => base.KeyDown -= x)
            .Select(e => e.EventArgs)
            .Select(e => e.Key switch
            {
                global::Avalonia.Input.Key.LeftCtrl => Interaction.UserInput.Key.Control,
                global::Avalonia.Input.Key.LeftShift => Interaction.UserInput.Key.Shift,
                global::Avalonia.Input.Key.Escape => Interaction.UserInput.Key.Escape,
                global::Avalonia.Input.Key.Z => Interaction.UserInput.Key.Z,
                global::Avalonia.Input.Key.R => Interaction.UserInput.Key.R,
                _ => Interaction.UserInput.Key.Unknown
            });

        KeyUp = Observable.FromEventPattern<EventHandler<KeyEventArgs>, KeyEventArgs>(x => base.KeyUp += x, x => base.KeyUp -= x)
            .Select(e => e.EventArgs)
            .Select(e => e.Key switch
            {
                global::Avalonia.Input.Key.LeftCtrl => Interaction.UserInput.Key.Control,
                global::Avalonia.Input.Key.LeftShift => Interaction.UserInput.Key.Shift,
                global::Avalonia.Input.Key.Escape => Interaction.UserInput.Key.Escape,
                global::Avalonia.Input.Key.Z => Interaction.UserInput.Key.Z,
                global::Avalonia.Input.Key.R => Interaction.UserInput.Key.R,
                _ => Interaction.UserInput.Key.Unknown
            });
    }




    private void BaseInteractiveControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        var nextRightButtonDown = point.Properties.IsRightButtonPressed;
        var nextLeftButtonDown = point.Properties.IsLeftButtonPressed;

        if (leftButtonIsDown && !nextLeftButtonDown)
        {
            leftButtonObservable.Next(nextLeftButtonDown);
            leftButtonIsDown = nextLeftButtonDown;
        }

        if (rightButtonIsDown && !nextRightButtonDown)
        {
            rightButtonObservable.Next(nextRightButtonDown);
            rightButtonIsDown = nextRightButtonDown;
        }
    }

    private void BaseInteractiveControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        var nextRightButtonDown = point.Properties.IsRightButtonPressed;
        var nextLeftButtonDown = point.Properties.IsLeftButtonPressed;

        if (!leftButtonIsDown && nextLeftButtonDown)
        {
            leftButtonObservable.Next(nextLeftButtonDown);
            leftButtonIsDown = nextLeftButtonDown;
        }

        if (!rightButtonIsDown && nextRightButtonDown)
        {
            rightButtonObservable.Next(nextRightButtonDown);
            rightButtonIsDown = nextRightButtonDown;
        }
    }

    /// <inheritdoc/>
    public void Refresh()
    {
        InvalidateVisual();
    }

    /// <inheritdoc/>
    public override void Render(DrawingContext drawingContext)
    {
        drawingContext.PushTransform(new Matrix(Zoom, 0, 0, Zoom, 0, 0));
        drawingContext.PushTransform(new Matrix(1, 0, 0, 1, TranslateX, TranslateY));

        foreach (var action in DrawActions)
        {
            action(drawingContext);
        }
    }
}