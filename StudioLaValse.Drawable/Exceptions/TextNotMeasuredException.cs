using StudioLaValse.Drawable.Text;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An exception that is thrown when the dimensions of a <see cref="DrawableText"/> is accessed before measuring it using a <see cref="IMeasureText"/>. 
    /// Call <see cref="DrawableText.Measure(IMeasureText)"/> before accessing it's dimensions.
    /// </summary>
    public class TextNotMeasuredException : Exception { }
}
