using Avalonia;
using Avalonia.Media;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudioLaValse.Drawable.Avalonia.Extensions;
internal static class GeometryExtensions
{
    public static Point ToPoint(this XY point)
    {
        return new Point(point.X, point.Y);
    }

    public static Color ToColor(this ColorARGB colorARGB)
    {
        var color = new Color((byte)colorARGB.Alpha, (byte)colorARGB.Red, (byte)colorARGB.Green, (byte)colorARGB.Blue);
        return color;
    }

    public static SolidColorBrush ToBrush(this ColorARGB colorARGB)
    {
        var color = colorARGB.ToColor();
        var brush = new SolidColorBrush(color);
        return brush;
    }

    public static Pen ToPen(this ColorARGB colorARGB, double thickness)
    {
        var brush = colorARGB.ToBrush();
        var pen = new Pen(brush, thickness);
        return pen;
    }

    public static Rect ToRect(this DrawableRectangle rectangle)
    {
        var rect = new Rect(new Point((int)rectangle.TopLeftX, (int)rectangle.TopLeftY), new Size((int)rectangle.Width, (int)rectangle.Height));
        return rect;
    }

    public static FormattedText ToFormattedText(this DrawableText text)
    {
        var brush = text.Color.ToBrush();
        var fontFamily = new FontFamily(text.FontFamily.Uri?.ToString() ?? text.FontFamily.Name);
        var formattedText = new FormattedText(text.Text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), text.FontSize, brush);
        return formattedText;
    }
}
