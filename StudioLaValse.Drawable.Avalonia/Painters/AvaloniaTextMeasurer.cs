using Avalonia.Media;
using StudioLaValse.Geometry;
using StudioLaValse.Drawable.Text;

namespace StudioLaValse.Drawable.Avalonia.Painters;

/// <summary>
/// A textmeasurer for Avalonia applications.
/// </summary>
public class AvaloniaTextMeasurer : IMeasureText
{
    /// <inheritdoc/>
    public XY Measure(string text, FontFamilyCore fontFamilyCore, double size)
    {
        var fontFamily = new FontFamily(fontFamilyCore.Uri, fontFamilyCore.Name);
        var formattedText = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), size, Brushes.White);
        var xy = new XY(formattedText.Width, formattedText.Height);
        return xy;
    }
}
