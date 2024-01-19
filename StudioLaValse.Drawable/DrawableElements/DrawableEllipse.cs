using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents a drawable ellipse.
    /// </summary>
    public class DrawableEllipse : BaseDrawableElement
    {
        /// <summary>
        /// The X-coordinate of the center of the ellipse.
        /// </summary>
        public double CenterX { get; }
        /// <summary>
        /// The Y-coordinate of the center of the ellipse.
        /// </summary>
        public double CenterY { get; }
        /// <summary>
        /// The width of the ellipse.
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// The height of the ellipse.
        /// </summary>
        public double Height { get; }
        /// <summary>
        /// The color of the ellipse.
        /// </summary>
        public ColorARGB Color { get; }
        /// <summary>
        /// The stroke color of the ellipse. May be null.
        /// </summary>
        public ColorARGB? StrokeColor { get; }
        /// <summary>
        /// The stroke thickness of the ellipse.
        /// </summary>
        public double StrokeWeight { get; }


        /// <summary>
        /// The primary constructor.
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="foreground"></param>
        /// <param name="stroke"></param>
        /// <param name="strokeWeight"></param>
        public DrawableEllipse(double centerX, double centerY, double width, double height, ColorARGB foreground, ColorARGB? stroke = null, double strokeWeight = 0)
        {
            CenterX = centerX;
            CenterY = centerY;
            Width = width;
            Height = height;
            Color = foreground;
            StrokeColor = stroke;
            StrokeWeight = strokeWeight;
        }


        /// <inheritdoc/>
        public override BoundingBox GetBoundingBox() =>
            new BoundingBox(
                CenterX - Width / 2,
                CenterX + Width / 2,
                CenterY - Height / 2,
                CenterY + Height / 2);
    }
}
