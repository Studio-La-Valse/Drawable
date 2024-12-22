using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using System;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.UserInput;

namespace StudioLaValse.Drawable.Interaction.ViewModels;

/// <summary>
/// A template viewmodel for a canvas control.
/// </summary>
public class CanvasViewModel : ViewModelBase
{
    /// <summary>
    /// The base bitmap painter.
    /// </summary>
    public virtual BaseBitmapPainter CanvasPainter { get; }

    /// <summary>
    /// A <see cref="BaseDrawableElement"/> emitter.
    /// </summary>
    public IObservable<BaseDrawableElement> ElementEmitter
    {
        get => GetValue(() => ElementEmitter);
        set => SetValue(() => ElementEmitter, value);
    }

    /// <summary>
    /// An input observer.
    /// </summary>
    public IInputObserver InputObserver
    {
        get => GetValue(() => InputObserver);
        set => SetValue(() => InputObserver, value);
    }

    /// <summary>
    /// A selection border.
    /// </summary>
    public IObservable<BoundingBox> SelectionBorder
    {
        get => GetValue(() => SelectionBorder);
        set => SetValue(() => SelectionBorder, value);
    }

    /// <summary>
    /// The zoom factor.
    /// </summary>
    public double Zoom
    {
        get => GetValue(() => Zoom);
        set => SetValue(() => Zoom, value);
    }

    /// <summary>
    /// The translate x factor.
    /// </summary>
    public double TranslateX
    {
        get => GetValue(() => TranslateX);
        set => SetValue(() => TranslateX, value);
    }

    /// <summary>
    /// Thg translate y factor.
    /// </summary>
    public double TranslateY
    {
        get => GetValue(() => TranslateY);
        set => SetValue(() => TranslateY, value);
    }

    /// <summary>
    /// The bounds.
    /// </summary>
    public BoundingBox Bounds
    {
        get => GetValue(() => Bounds);
        set => SetValue(() => Bounds, value);
    }

    /// <summary>
    /// Enables pan.
    /// </summary>
    public bool EnablePan
    {
        get => GetValue(() => EnablePan);
        set => SetValue(() => EnablePan, value);
    }


    /// <summary>
    /// Enables zoom.
    /// </summary>
    public bool EnableZoom
    {
        get => GetValue(() => EnableZoom);
        set => SetValue(() => EnableZoom, value);
    }

    /// <summary>
    /// The default constructor.
    /// </summary>
    public CanvasViewModel()
    {
        var emitter = new DrawableElementEmitter();
        CanvasPainter = new DrawableElementEmitterBitmapPainter(emitter);

        ElementEmitter = emitter;

        InputObserver = new BaseInputObserver();

        Zoom = 1;
        EnablePan = true;
        EnableZoom = true;
    }

    /// <summary>
    /// Centers the content around a content wrapper.
    /// </summary>
    /// <param name="baseContentWrapper"></param>
    public void CenterContent(BaseContentWrapper baseContentWrapper)
    {
        var boundingBox = baseContentWrapper.BoundingBox();

        if (boundingBox.Width == 0 || boundingBox.Height == 0)
        {
            return;
        }

        // Step 0: calculate view center
        var viewCenterX = Bounds.MinPoint.X + Bounds.Width / 2;
        var viewCenterY = Bounds.MinPoint.Y + Bounds.Height / 2;

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
