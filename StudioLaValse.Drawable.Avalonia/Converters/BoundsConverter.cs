using Avalonia.Data.Converters;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioLaValse.Drawable.Avalonia.Converters;

/// <summary>
/// Converters <see cref="global::Avalonia.Rect"/> to <see cref="Geometry.BoundingBox"/>.
/// </summary>
public class BoundsConverter : IValueConverter
{
    /// <summary>
    /// Converts <see cref="Geometry.BoundingBox"/> to <see cref="global::Avalonia.Rect"/>.
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
            return new global::Avalonia.Rect(sourceRect.MinPoint.X, sourceRect.MinPoint.Y, sourceRect.Width, sourceRect.Height);
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    /// <summary>
    /// Converters <see cref="Geometry.BoundingBox"/> to <see cref="global::Avalonia.Rect"/> .
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
        if (value is global::Avalonia.Rect sourceRect)
        {
            return new Geometry.BoundingBox(sourceRect.X, sourceRect.X + sourceRect.Width, sourceRect.Y, sourceRect.Y + sourceRect.Height);
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}