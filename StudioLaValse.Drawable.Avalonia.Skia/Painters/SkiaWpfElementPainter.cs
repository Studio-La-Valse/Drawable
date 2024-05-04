using SkiaSharp;
using StudioLaValse.Drawable.Avalonia.Skia.Controls;
using StudioLaValse.Drawable.Skia.BitmapPainters;
using StudioLaValse.Geometry;
using StudioLaValse.Drawable.Avalonia.Extensions;

namespace StudioLaValse.Drawable.Avalonia.Skia.Painters;

public class SkiaWpfElementPainter : BaseSkiaBitmapPainter
{
    private readonly SKElementUserControl uc;


    public SkiaWpfElementPainter(SKElementUserControl uc)
    {
        this.uc = uc;
    }

    protected override List<Action<SKCanvas>> Cache => uc.Cache;

    public override void DrawBackground(ColorARGB color)
    {
        uc.Background = color.ToBrush();
    }

    public override void FinishDrawing()
    {
        uc.SKElement.InvalidateVisual();
    }
}
