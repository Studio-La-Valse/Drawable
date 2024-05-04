using Avalonia.Controls;
using Avalonia.Input;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Reactive.Linq;
using Avalonia.Media;
using System.Diagnostics;

namespace StudioLaValse.Drawable.Avalonia.Controls;

internal class DefaultUnsubscriber<TEntity> : IDisposable
{
    private readonly IObserver<TEntity> observer;
    private readonly ISet<IObserver<TEntity>> observers;

    public DefaultUnsubscriber(IObserver<TEntity> observer, ISet<IObserver<TEntity>> observers)
    {
        this.observer = observer;
        this.observers = observers;
    }

    public void Dispose()
    {
        observers.Remove(observer);
    }
}

internal class ButtonObservable : IObservable<bool>
{
    private readonly ISet<IObserver<bool>> observers = new HashSet<IObserver<bool>>();
    
    public ButtonObservable()
    {
        
    }

    public void Next(bool value)
    {
        foreach(var  observer in this.observers)
        {
            observer.OnNext(value);
        }
    }

    public IDisposable Subscribe(IObserver<bool> observer)
    {
        observers.Add(observer);
        return new DefaultUnsubscriber<bool>(observer, observers);
    }
}

public abstract partial class BaseInteractiveControl : UserControl, IInteractiveCanvas
{
    private readonly ButtonObservable leftButtonObservable = new ButtonObservable();
    private readonly ButtonObservable rightButtonObservable = new ButtonObservable();
    public BaseInteractiveControl()
    {
        Focusable = true;
        Background = Brushes.Transparent;
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
                _ => Interaction.UserInput.Key.Unknown
            });

        KeyUp = Observable.FromEventPattern<EventHandler<KeyEventArgs>, KeyEventArgs>(x => base.KeyUp += x, x => base.KeyUp -= x)
            .Select(e => e.EventArgs)
            .Select(e => e.Key switch
            {
                global::Avalonia.Input.Key.LeftCtrl => Interaction.UserInput.Key.Control,
                global::Avalonia.Input.Key.LeftShift => Interaction.UserInput.Key.Shift,
                global::Avalonia.Input.Key.Escape => Interaction.UserInput.Key.Escape,
                _ => Interaction.UserInput.Key.Unknown
            });

        MouseLeftButtonDown.Subscribe(v => Debug.WriteLine($"left button down: {v}"));
        MouseRightButtonDown.Subscribe(v => Debug.WriteLine($"right button down: {v}"));
    }

    private bool leftButtonIsDown;
    private bool rightButtonIsDown;
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

    public double Zoom { get; set; } = 1;
    public double TranslateX { get; set; } = 0;
    public double TranslateY { get; set; } = 0;
    public double ViewBoxWidth => double.IsNormal(Width) ? Width : 0;
    public double ViewBoxHeight => double.IsNormal(Height) ? Width : 0;


    public IObservable<XY> MouseMove { get; }
    public IObservable<bool> MouseLeftButtonDown { get; }
    public IObservable<bool> MouseRightButtonDown { get; }
    public IObservable<double> MouseWheel { get; }
    new public IObservable<Interaction.UserInput.Key> KeyDown { get; }
    new public IObservable<Interaction.UserInput.Key> KeyUp { get; }

    public abstract void Refresh();
}