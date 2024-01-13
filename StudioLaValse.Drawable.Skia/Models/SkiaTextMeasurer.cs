using SkiaSharp;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Skia.Models
{
    /// <summary>
    /// The default text measurer for all Skia implementations.
    /// </summary>
    public class SkiaTextMeasurer : IMeasureText
    {
        public XY Measure(string text, FontFamilyCore fontFamily, double size)
        {
            var typeFace = SKTypeface.FromFamilyName(fontFamily.Name);
            var font = new SKFont(typeFace, (float)size)
            {
                Subpixel = true
            };
            var bounds = new SKRect();
            var paint = new SKPaint(font)
            {
                TextAlign = SKTextAlign.Left
            };
            var width = paint.MeasureText(text, ref bounds);

            return new XY(Math.Abs(width), font.Spacing);
        }
    }
}
