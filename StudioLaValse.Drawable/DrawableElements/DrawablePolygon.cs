using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An implementation of the <see cref="BaseDrawableElement"/> that represents a polygon.
    /// </summary>
    public class DrawablePolygon : BaseDrawableElement
    {
        public IEnumerable<XY> Points { get; }
        public ColorARGB? Fill { get; }
        public ColorARGB? Color { get; }
        public double StrokeWeight { get; }

        public DrawablePolygon(IEnumerable<XY> points, ColorARGB? fill = null, ColorARGB? strokeColor = null, double strokeWeight = 0)
        {
            Points = points;
            Fill = fill;
            Color = strokeColor;
            StrokeWeight = strokeWeight;
        }

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
    }
}
