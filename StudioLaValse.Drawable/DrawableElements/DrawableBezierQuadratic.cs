using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An drawable bezier curve as an extension of a drawable polyline.
    /// </summary>
    public class DrawableBezierQuadratic : BaseDrawableElement
    {
        private readonly XY first;
        private readonly XY second;
        private readonly XY third;
        private readonly ColorARGB strokeColor;
        private readonly double strokeWeight;

        /// <summary>
        /// The points of the polyline.
        /// </summary>
        public IEnumerable<XY> Points => [first, second, third];
        /// <summary>
        /// The color of the polyline.
        /// </summary>
        public ColorARGB Color => strokeColor;
        /// <summary>
        /// The thickness of the polyline.
        /// </summary>
        public double StrokeWeight => strokeWeight;

        /// <summary>
        /// The first point.
        /// </summary>
        public XY First => this.first;

        /// <summary>
        /// The second point.
        /// </summary>
        public XY Second => this.second;

        /// <summary>
        /// The third point.
        /// </summary>
        public XY Third => this.third;

        /// <summary>
        /// The primary constructor.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <param name="strokeColor"></param>
        /// <param name="strokeWeight"></param>
        public DrawableBezierQuadratic(XY first, XY second, XY third, ColorARGB strokeColor, double strokeWeight) 
        {
            this.first = first;
            this.second = second;
            this.third = third;

            this.strokeColor = strokeColor;
            this.strokeWeight = strokeWeight;
        }

        /// <inheritdoc/>
        public override BoundingBox BoundingBox()
        {
            var simplified = new QuadraticBezierSegment(first, second, third).ToPolyline(10);
            var minX = 0d;
            var maxX = 0d;
            var minY = 0d;
            var maxY = 0d;

            var firstPoint = true;
            foreach (var point in simplified.Points)
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
            return new QuadraticBezierSegment(first, second, third).ClosestPoint(other);
        }

        /// <inheritdoc/>
        public override XY ClosestPointShape(XY other)
        {
            return new QuadraticBezierSegment(first, second, third).ClosestPoint(other);
        }
    }
}
