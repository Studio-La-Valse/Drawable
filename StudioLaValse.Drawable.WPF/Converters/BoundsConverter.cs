using System.Globalization;
using System.Windows.Data;

namespace StudioLaValse.Drawable.WPF.Converters;

/// <summary>
/// Converters <see cref="System.Windows.Rect"/> to <see cref="Geometry.BoundingBox"/>.
/// </summary>
public class BoundsConverter : IValueConverter
{
    /// <summary>
    /// Converts <see cref="Geometry.BoundingBox"/> to <see cref="System.Drawing.Rectangle"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Geometry.BoundingBox sourceRect)
        {
            return new System.Windows.Rect(sourceRect.MinPoint.X, sourceRect.MinPoint.Y, sourceRect.Width, sourceRect.Height);
        }

        // converter used for the wrong type
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converters <see cref="Geometry.BoundingBox"/> to <see cref="System.Windows.Rect"/> .
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public object ConvertBack(object? value, Type targetType,
                                object? parameter, CultureInfo culture)
    {
        if (value is System.Windows.Rect sourceRect)
        {
            return new Geometry.BoundingBox(sourceRect.X, sourceRect.X + sourceRect.Width, sourceRect.Y, sourceRect.Y + sourceRect.Height);
        }

        // converter used for the wrong type
        throw new NotImplementedException();
    }
}