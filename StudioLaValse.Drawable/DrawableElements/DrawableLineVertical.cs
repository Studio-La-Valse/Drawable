using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An extension of the <see cref="DrawableLine"/> that is vertical.
    /// </summary>
    public sealed class DrawableLineVertical : DrawableLine
    {
        public DrawableLineVertical(double horizontalPosition, double verticalPosition, double lengthVisualDown, double thickness, ColorARGB color) :
            base(horizontalPosition, horizontalPosition, verticalPosition, verticalPosition + lengthVisualDown, color, thickness)
        {

        }
    }
}
