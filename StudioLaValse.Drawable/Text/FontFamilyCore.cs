using StudioLaValse.Drawable.BitmapPainters;

namespace StudioLaValse.Drawable.Text
{
    /// <summary>
    /// Represents a class that contains all necessary info the locate a font by a platform specific <see cref="BaseBitmapPainter"/>.
    /// </summary>
    public class FontFamilyCore
    {
        public string Name { get; }

        public FontFamilyCore(string name)
        {
            Name = name;
        }
    }
}
