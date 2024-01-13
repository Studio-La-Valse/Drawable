using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Text
{
    /// <summary>
    /// An interface that requires a platform specific implementation to measure text.
    /// </summary>
    public interface IMeasureText
    {
        /// <summary>
        /// Measures the specified text. The X and Y fields of the returned <see cref="XY"/> instance describe the with and height of the text respectively.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontFamily"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public XY Measure(string text, FontFamilyCore fontFamily, double size);
    }
}
