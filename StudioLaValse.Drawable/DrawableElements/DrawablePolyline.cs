using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An implementation of the <see cref="BaseDrawableElement"/> that represents a drawable polyline.
    /// </summary>
    public class DrawablePolyline : BaseDrawableElement
    {
        /// <summary>
        /// The points of the polyline.
        /// </summary>
        public IEnumerable<XY> Points { get; }
        /// <summary>
        /// The color of the polyline.
        /// </summary>
        public ColorARGB Color { get; }
        /// <summary>
        /// The thickness of the polyline.
        /// </summary>
        public double StrokeWeight { get; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="strokeColor"></param>
        /// <param name="strokeWeight"></param>
        public DrawablePolyline(IEnumerable<XY> points, ColorARGB strokeColor, double strokeWeight)
        {
            Points = points;
            Color = strokeColor;
            StrokeWeight = strokeWeight;
        }

        /// <inheritdoc/>
        public override BoundingBox GetBoundingBox()
        {
            var minX = 0d;
            var maxX = 0d;
            var minY = 0d;
            var maxY = 0d;

            var firstPoint = true;
            foreach (var point in Points)
            {
                if (firstPoint)
                {
                    minX = point.X;
                    maxX = point.X;
                    minY = point.Y;
                    maxY = point.Y;
                    firstPoint = false;
                    continue;
                }

                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            return new BoundingBox(minX, maxX, minY, maxY);
        }

        /// <inheritdoc/>
        public override XY ClosestPointEdge(XY other)
        {
            return new Polyline(Points).ClosestPoint(other);
        }

        /// <inheritdoc/>
        public override XY ClosestPointShape(XY other)
        {
            return new Polyline(Points).ClosestPoint(other);
        }
    }
}
