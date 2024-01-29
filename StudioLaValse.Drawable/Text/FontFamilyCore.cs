using StudioLaValse.Drawable.BitmapPainters;

namespace StudioLaValse.Drawable.Text
{
    /// <summary>
    /// Represents a class that contains all necessary info the locate a font by a platform specific <see cref="BaseBitmapPainter"/>.
    /// </summary>
    public class FontFamilyCore
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        public FontFamilyCore(string name, Uri? uri = null)
        {
            Uri = uri;
            Name = name;
        }

        /// <summary>
        /// The Uri of the font. Optional.
        /// </summary>
        public Uri? Uri { get; }
        /// <summary>
        /// The name of the font.
        /// </summary>
        public string Name { get; }
    }
}
