using StudioLaValse.Drawable.BitmapPainters;

namespace StudioLaValse.Drawable.Text
{
    /// <summary>
    /// Represents a class that contains all necessary info the locate a font by a platform specific <see cref="BaseBitmapPainter"/>.
    /// </summary>
    public class FontFamilyCore
    {
        /// <summary>
        /// The name of the font.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The primary constructor.
        /// </summary>
        /// <param name="name"></param>
        public FontFamilyCore(string name)
        {
            Name = name;
        }
    }
}
