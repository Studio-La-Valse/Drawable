using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Skia.Helpers;
using Avalonia.Platform;
using SkiaSharp;
using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.Skia.Models;
using StudioLaValse.Drawable.Text;

namespace StudioLaValse.Drawable.Avalonia.Skia.Controls;
public partial class SKElementUserControl : BaseInteractiveControl
{
    public SKElementUserControl()
    {
        InitializeComponent();
    }

    public List<Action<SKCanvas>> Cache = new();

    public override void Refresh() => this.InvalidateVisual();

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

    public override void Render(DrawingContext drawingContext)
    {
        drawingContext.PushTransform(new Matrix(Zoom, 0, 0, Zoom, 0, 0));
        drawingContext.PushTransform(new Matrix(1, 0, 0, 1, TranslateX, TranslateY));

        var drawAction = new SkiaDrawOperation();
        drawingContext.Custom(drawAction);
    }
}

internal class SkiaDrawOperation : ICustomDrawOperation
{
    public Rect Bounds => throw new NotImplementedException();

    public SkiaDrawOperation()
    {
        
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public bool Equals(ICustomDrawOperation? other)
    {
        throw new NotImplementedException();
    }

    public bool HitTest(Point p)
    {
        throw new NotImplementedException();
    }

    public void Render(ImmediateDrawingContext context)
    {
        var skia = context.GetFeature<ISkiaSharpApiLeaseFeature>();
        using (var lease = skia.Lease())
        {
            SKCanvas canvas = lease.SkCanvas;
            if (canvas != null) RenderAction?.Invoke(canvas);
        }
    }
}
