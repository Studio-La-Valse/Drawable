using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.Extensions;
using StudioLaValse.Geometry;
using System.Windows.Media;

namespace StudioLaValse.Drawable.WPF.Text
{
    /// <summary>
    /// The default WPF text measurer.
    /// </summary>
    public class WPFTextMeasurer : IMeasureText
    {
        public XY Measure(string text, FontFamilyCore fontFamily, double size)
        {
            var formattedText = text.AsFormattedText(fontFamily.ToFontFamily(), size);
            var textSize = formattedText.MeasureTextSize();
            return textSize;
        }
    }
}
