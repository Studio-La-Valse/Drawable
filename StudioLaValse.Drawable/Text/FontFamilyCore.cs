using StudioLaValse.Drawable.BitmapPainters;

namespace StudioLaValse.Drawable.Text
{
    /// <summary>
    /// Represents a class that contains all necessary info the locate a font by a platform specific <see cref="BaseBitmapPainter"/>.
    /// </summary>
    public class FontFamilyCore
    {
        /// <summary>
        /// Construct a FontFamilyCore from a Uri and a name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        public FontFamilyCore(Uri uri, string name)
        {
            Uri = uri;
            Name = name;
        }
        /// <summary>
        /// Construct a FontFamilyCore from a name.
        /// </summary>
        /// <param name="name"></param>
        public FontFamilyCore(string name)
        {
            Uri = null;
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
