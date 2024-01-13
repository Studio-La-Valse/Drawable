using SkiaSharp;
using StudioLaValse.Drawable.Skia.BitmapPainters;
using StudioLaValse.Drawable.WPF.Extensions;
using StudioLaValse.Drawable.WPF.Skia.UserControls;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.WPF.Skia.BitmapPainters
{
    /// <summary>
    /// The default Skia bitmap painter for WPF purposes.
    /// </summary>
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
            uc.Background = color.ToWindowsBrush();
        }

        public override void FinishDrawing()
        {
            uc.SKElement.InvalidateVisual();
        }
    }
}
