using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An extension the the drawable line that is horizontal.
    /// </summary>
    public sealed class DrawableLineHorizontal : DrawableLine
    {
        public DrawableLineHorizontal(double heightOnCanvas, double horizontalPosition, double length, double thickness, ColorARGB color) :
            base(horizontalPosition, horizontalPosition + length, heightOnCanvas, heightOnCanvas, color, thickness)
        {

        }
    }
}
