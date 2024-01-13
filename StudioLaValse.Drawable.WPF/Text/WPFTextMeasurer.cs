using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.Extensions;
using StudioLaValse.Geometry;
using System.Windows.Media;

namespace StudioLaValse.Drawable.WPF.Text
{
    /// <summary>
    /// The default WPF text measurer. Set this instance to <see cref="ExternalTextMeasure.TextMeasurer"/> to provide text measuring capabilities in WPF applications.
    /// </summary>
    public class WPFTextMeasurer : IMeasureText
    {
        public XY Measure(string text, FontFamilyCore fontFamily, double size)
        {
            var formattedText = text.AsFormattedText(new FontFamily(fontFamily.Name), size);
            var textSize = formattedText.MeasureTextSize();
            return textSize;
        }
    }
}
