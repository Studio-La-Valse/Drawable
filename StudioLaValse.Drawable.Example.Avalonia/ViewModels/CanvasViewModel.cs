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
using StudioLaValse.Drawable.ContentWrappers;

namespace StudioLaValse.Drawable.Example.Avalonia.ViewModels;

public class CanvasViewModel : ViewModelBase
{
    private readonly BaseBitmapPainter canvasPainter;

    public BaseBitmapPainter BaseBitmapPainter => canvasPainter;

    public IObservable<BaseDrawableElement> ElementEmitter
    {
        get => GetValue(() => ElementEmitter);
        set => SetValue(() => ElementEmitter, value);
    }
    public IBehavior Pipe
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

        Zoom = 1;
    }

    public void CenterContent(BaseContentWrapper baseContentWrapper) 
    {
        var boundingBox = baseContentWrapper.BoundingBox();

        if (boundingBox.Width == 0 || boundingBox.Height == 0)
        {
            return;
        }

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
    public static IBehavior UndoRedo(this IBehavior nextPipe, ICommandManager commandManager, INotifyEntityChanged<PersistentElement> notifyEntityChanged)
    {
        return new UndoRedoPipe(nextPipe, commandManager, notifyEntityChanged);
    }
}

internal class UndoRedoPipe : IBehavior
{
    private readonly IBehavior next;
    private readonly ICommandManager commandManager;
    private readonly INotifyEntityChanged<PersistentElement> notifyEntityChanged;
    private bool controlDown = false;

    public UndoRedoPipe(IBehavior next, ICommandManager commandManager, INotifyEntityChanged<PersistentElement> notifyEntityChanged)
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
