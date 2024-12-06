using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An implementation of the <see cref="BaseDrawableElement"/> that represents a polygon.
    /// </summary>
    public class DrawablePolygon : BaseDrawableElement
    {
        /// <summary>
        /// The points of the polygon.
        /// </summary>
        public IEnumerable<XY> Points { get; }
        /// <summary>
        /// The fill color of the polygon.
        /// </summary>
        public ColorARGB? Fill { get; }
        /// <summary>
        /// The stroke color of the polygon.
        /// </summary>
        public ColorARGB? Color { get; }
        /// <summary>
        /// The stroke weight of the polygon.
        /// </summary>
        public double StrokeWeight { get; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="fill"></param>
        /// <param name="strokeColor"></param>
        /// <param name="strokeWeight"></param>
        public DrawablePolygon(IEnumerable<XY> points, ColorARGB? fill = null, ColorARGB? strokeColor = null, double strokeWeight = 0)
        {
            Points = points;
            Fill = fill;
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
            return new Polygon(Points).ClosestPointEdge(other);
        }

        /// <inheritdoc/>
        public override XY ClosestPointShape(XY other)
        {
            return new Polygon(Points).ClosestPointShape(other);
        }
    }
}
