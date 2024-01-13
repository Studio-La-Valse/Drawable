using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents a drawable ellipse.
    /// </summary>
    public class DrawableEllipse : BaseDrawableElement
    {
        public double CenterX { get; }
        public double CenterY { get; }
        public double Width { get; }
        public double Height { get; }

        public ColorARGB Color { get; }
        public ColorARGB? StrokeColor { get; }

        public double StrokeWeight { get; }


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

        public override BoundingBox GetBoundingBox() =>
            new BoundingBox(
                CenterX - Width / 2,
                CenterX + Width / 2,
                CenterY - Height / 2,
                CenterY + Height / 2);
    }
}
