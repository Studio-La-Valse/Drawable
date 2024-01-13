using SkiaSharp;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Skia.Extensions
{
    public static class SkiaExtensions
    {
        public static SKPoint ToSkiaPoint(this XY xy) => new SKPoint((float)xy.X, (float)xy.Y);
    }
}
