using StudioLaValse.CommandManager;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Avalonia.Models;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Extensions;

namespace StudioLaValse.Drawable.Example.Avalonia.ViewModels;

public class Unsubscriber<T> : IDisposable
{
    private readonly ICollection<System.IObserver<T>> _observers;
    private readonly System.IObserver<T> _observer;

    private Unsubscriber(ICollection<System.IObserver<T>> observers, System.IObserver<T> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        _observers.Remove(_observer);
    }

    public static IDisposable SubscribeOrThrow(HashSet<System.IObserver<T>> observers, System.IObserver<T> observer)
    {
        if (!observers.Add(observer))
        {
            throw new InvalidOperationException();
        }

        return new Unsubscriber<T>(observers, observer);
    }
}


public class DrawableElementEmitter : System.IObservable<BaseDrawableElement>
{
    private readonly HashSet<System.IObserver<BaseDrawableElement>> observers = [];
    public DrawableElementEmitter()
    {

    }

    public void Emit(BaseDrawableElement baseDrawableElement)
    {
        foreach (var subscriber in observers)
        {
            subscriber.OnNext(baseDrawableElement);
        }
    }

    public void Complete()
    {
        foreach (var subscriber in observers)
        {
            subscriber.OnCompleted();
        }
    }

    public IDisposable Subscribe(System.IObserver<BaseDrawableElement> observer)
    {
        return Unsubscriber<BaseDrawableElement>.SubscribeOrThrow(observers, observer);
    }
}

public class InvalidatedElementDispatcher : IObserver<InvalidationRequest<PersistentElement>>
{
    private readonly SceneManager<PersistentElement, int> sceneManager;
    private readonly BaseBitmapPainter baseBitmapPainter;

    public InvalidatedElementDispatcher(SceneManager<PersistentElement, int> sceneManager, BaseBitmapPainter baseBitmapPainter)
    {
        this.sceneManager = sceneManager;
        this.baseBitmapPainter = baseBitmapPainter;
    }
    public void OnCompleted()
    {
        sceneManager.RenderChanges(baseBitmapPainter);
    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnNext(InvalidationRequest<PersistentElement> value)
    {
        sceneManager.AddToQueue(value);
    }
}

public class DrawableElementEmitterBitmapPainter : BaseBitmapPainter
{
    private readonly DrawableElementEmitter drawableElementEmitter;

    public DrawableElementEmitterBitmapPainter(DrawableElementEmitter drawableElementEmitter)
    {
        this.drawableElementEmitter = drawableElementEmitter;
    }

    public override void DrawBackground(ColorARGB color)
    {

    }

    public override void DrawElement(BaseDrawableElement element)
    {
        drawableElementEmitter.Emit(element);
    }

    public override void FinishDrawing()
    {
        drawableElementEmitter.Complete();
    }

    public override void InitDrawing()
    {

    }
}

public class CanvasViewModel : ViewModelBase
{
    private bool initialized = false;
    private SceneManager<PersistentElement, int>? sceneManager;
    private readonly BaseBitmapPainter canvasPainter;

    public BaseBitmapPainter BaseBitmapPainter => canvasPainter;
    public SceneManager<PersistentElement, int>? SceneManager 
    { 
        get => sceneManager; 
        set
        {
            sceneManager = value;
            sceneManager?.Rerender(canvasPainter);
        }
    }

    public IObservable<BaseDrawableElement> ElementEmitter
    {
        get => GetValue(() => ElementEmitter);
        set => SetValue(() => ElementEmitter, value);
    }
    public IPipe Pipe
    {
        get => GetValue(() => Pipe);
        set => SetValue(() => Pipe, value);
    }
    public ObservableBoundingBox SelectionBorder
    {
        get => GetValue(() => SelectionBorder);
        set => SetValue(() => SelectionBorder, value);
    }
    public double Zoom
    {
        get => GetValue(() => Zoom);
        set => SetValue(() => Zoom, value);
    }
    public double TranslateX
    {
        get => GetValue(() => TranslateX);
        set => SetValue(() => TranslateX, value);
    }
    public double TranslateY
    {
        get => GetValue(() => TranslateY);
        set => SetValue(() => TranslateY, value);
    }
    public global::Avalonia.Rect Bounds
    {
        get => GetValue(() => Bounds);
        set => SetValue(() => Bounds, value);
    }

    public bool EnablePan
    {
        get => GetValue(() =>  EnablePan);
        set => SetValue(() => EnablePan, value);
    }

    public bool EnableZoom
    {
        get => GetValue(() => EnableZoom);
        set => SetValue(() => EnableZoom, value);
    }

    public CanvasViewModel()
    {
        var emitter = new DrawableElementEmitter();
        canvasPainter = new DrawableElementEmitterBitmapPainter(emitter);

        ElementEmitter = emitter;

        SelectionBorder = new ObservableBoundingBox();
        var commandManager = StudioLaValse.CommandManager.CommandManager.CreateGreedy();

        Zoom = 1;
    }

    public void CenterContent()
    {
        if(sceneManager is null)
        {
            return;
        }

        var boundingBox = new BoundingBox(sceneManager.VisualParents
            .SelectMany(e => e.GetDrawableElements().Select(e => e.GetBoundingBox())));

        // Step 0: calculate view center
        var viewCenterX = Bounds.X + Bounds.Width / 2;
        var viewCenterY = Bounds.Y + Bounds.Height / 2;

        // Step 1: Calculate the current center points
        var contentCenterX = boundingBox.MinPoint.X + boundingBox.Width / 2.0;
        var contentCenterY = boundingBox.MinPoint.Y + boundingBox.Height / 2.0;

        // Step 2: Move content to center at (0,0)
        var initialTranslateX = -contentCenterX;
        var initialTranslateY = -contentCenterY;

        // Apply the initial translation to move the content to (0,0)
        TranslateX = initialTranslateX;
        TranslateY = initialTranslateY;

        // Step 3: Calculate the zoom factor to fit the content within the view
        var scaleX = Bounds.Width / boundingBox.Width;
        var scaleY = Bounds.Height / boundingBox.Height;
        var zoomFactor = Math.Min(scaleX, scaleY);

        // Apply the zoom factor
        Zoom = zoomFactor;

        // Move the content back to the center
        TranslateX += viewCenterX / zoomFactor;
        TranslateY += viewCenterY / zoomFactor;
    }
}


public static class PipeExtensions
{
    public static IPipe UndoRedo(this IPipe nextPipe, ICommandManager commandManager, INotifyEntityChanged<PersistentElement> notifyEntityChanged)
    {
        return new UndoRedoPipe(nextPipe, commandManager, notifyEntityChanged);
    }
}

internal class UndoRedoPipe : IPipe
{
    private readonly IPipe next;
    private readonly ICommandManager commandManager;
    private readonly INotifyEntityChanged<PersistentElement> notifyEntityChanged;
    private bool controlDown = false;

    public UndoRedoPipe(IPipe next, ICommandManager commandManager, INotifyEntityChanged<PersistentElement> notifyEntityChanged)
    {
        this.next = next;
        this.commandManager = commandManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }

    public void HandleLeftMouseButtonDown()
    {
        next.HandleLeftMouseButtonDown();
    }

    public void HandleLeftMouseButtonUp()
    {
        next.HandleLeftMouseButtonUp();
    }

    public void HandleMouseWheel(double delta)
    {
        next.HandleMouseWheel(delta);
    }

    public void HandleRightMouseButtonDown()
    {
        next.HandleRightMouseButtonDown();
    }

    public void HandleRightMouseButtonUp()
    {
        next.HandleRightMouseButtonUp();
    }

    public void HandleSetMousePosition(XY position)
    {
        next.HandleSetMousePosition(position);
    }

    public void KeyDown(Interaction.UserInput.Key key)
    {
        if (key == Interaction.UserInput.Key.Control)
        {
            controlDown = true;
        }

        next.KeyDown(key);
    }

    public void KeyUp(Interaction.UserInput.Key key)
    {
        if (key == Interaction.UserInput.Key.Control)
        {
            controlDown = false;
        }

        if (controlDown && key == Interaction.UserInput.Key.Z)
        {
            try
            {
                commandManager.Undo();
                notifyEntityChanged.RenderChanges();
            }
            catch
            {

            }
        }

        if (controlDown && key == Interaction.UserInput.Key.R)
        {
            try
            {
                commandManager.Redo();
                notifyEntityChanged.RenderChanges();
            }
            catch
            {

            }
        }

        next.KeyUp(key);
    }
}
