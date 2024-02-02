using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents a drawable line. 
    /// </summary>
    public class DrawableLine : BaseDrawableElement
    {
        /// <summary>
        /// The color of the line.
        /// </summary>
        public ColorARGB Color { get; }

        /// <summary>
        /// The X-coordinate of the first point of the line.
        /// </summary>
        public double X1 { get; }
        /// <summary>
        /// The x-coordinate of the second point of the line.
        /// </summary>
        public double X2 { get; }
        /// <summary>
        /// The y-coordianate of the first point of the line.
        /// </summary>
        public double Y1 { get; }
        /// <summary>
        /// The y-coordianate of the second point of the line.
        /// </summary>
        public double Y2 { get; }
        /// <summary>
        /// The thickness of the line.
        /// </summary>
        public double Thickness { get; }
        /// <summary>
        /// The topleft point of the bounding box of the line.
        /// </summary>
        public XY TopLeft => new XY(Math.Min(X1, X2), Math.Min(Y1, Y2));
        /// <summary>
        /// The bottom right point of the bounding box of the line.
        /// </summary>
        public XY BottomRight => new XY(Math.Max(X1, X2), Math.Max(Y1, Y2));


        /// <summary>
        /// Construct a line from coordinates.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        public DrawableLine(double x1, double x2, double y1, double y2, ColorARGB color, double thickness)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            Thickness = thickness;
            Color = color;
        }

        /// <summary>
        /// Construct a line from two points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        public DrawableLine(XY start, XY end, ColorARGB color, double thickness)
        {
            X1 = start.X;
            X2 = end.X;
            Y1 = start.Y;
            Y2 = end.Y;
            Thickness = thickness;
            Color = color;
        }

        /// <summary>
        /// Construct a line from a geometric line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="thickness"></param>
        /// <param name="color"></param>
        public DrawableLine(Line line, double thickness, ColorARGB color)
        {
            X1 = line.Start.X;
            X2 = line.End.X;
            Y1 = line.Start.Y;
            Y2 = line.End.Y;
            Thickness = thickness;
            Color = color;
        }

        /// <summary>
        /// Thicken this line to a trapezoid shape.
        /// </summary>
        /// <returns></returns>
        public DrawableTrapezoid ToTrapezoid() =>
            new DrawableTrapezoid(new XY(X1, Y1), new XY(X2, Y2), Thickness, Color);

        /// <inheritdoc/>
        public override BoundingBox GetBoundingBox() =>
            new BoundingBox(X1 - Thickness / 2, X2 + Thickness / 2, Y1 - Thickness / 2, Y2 + Thickness / 2);

        /// <summary>
        /// Calculate the distance between the line and this point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetDistance(XY point)
        {
            return new Line(X1, Y1, X2, Y2).Distance(point);
        }
    }
}
