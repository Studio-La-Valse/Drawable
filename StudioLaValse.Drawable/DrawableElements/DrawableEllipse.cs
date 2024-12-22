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
        public override BoundingBox BoundingBox() =>
            new BoundingBox(
                CenterX - Width / 2,
                CenterX + Width / 2,
                CenterY - Height / 2,
                CenterY + Height / 2);

        /// <inheritdoc/>
        public override XY ClosestPointEdge(XY other)
        {
            var (x, y) = ClosestPointOnBoundary(other.X, other.Y);
            var xy = new XY(x, y);
            return xy;
        }

        /// <inheritdoc/>
        public override XY ClosestPointShape(XY other)
        {
            var (x, y) = ClosestPointInside(other.X, other.Y);
            var xy = new XY(x, y);
            return xy;
        }

        private (double x, double y) ClosestPointInside(double pointX, double pointY)
        {
            var a = Width / 2.0;
            var b = Height / 2.0;

            // Translate point and ellipse to the origin
            var dx = pointX - CenterX;
            var dy = pointY - CenterY;

            // Calculate the distance from the point to the center
            var distance = Math.Sqrt(dx * dx + dy * dy);

            // Calculate the scaled coordinates
            var scaledX = a * dx / Math.Max(a, distance);
            var scaledY = b * dy / Math.Max(b, distance);

            // Check if the point is inside the ellipse
            var ellipseEq = (scaledX * scaledX) / (a * a) + (scaledY * scaledY) / (b * b);

            // If point is inside, return the point
            if (ellipseEq <= 1)
            {
                return (pointX, pointY);
            }

            // If point is outside, project onto ellipse
            var scale = Math.Sqrt(1 / ellipseEq);
            var closestX = CenterX + scaledX * scale;
            var closestY = CenterY + scaledY * scale;

            return (closestX, closestY);
        }

        private (double x, double y) ClosestPointOnBoundary(double pointX, double pointY)
        {
            var a = Width / 2.0;
            var b = Height / 2.0;

            // Translate point and ellipse to the origin
            var dx = pointX - CenterX;
            var dy = pointY - CenterY;

            // Calculate the angle of the vector from the center to the point
            var angle = Math.Atan2(dy, dx);

            // Calculate the coordinates of the closest point on the boundary
            var closestX = CenterX + a * Math.Cos(angle);
            var closestY = CenterY + b * Math.Sin(angle);

            return (closestX, closestY);
        }
    }
}

