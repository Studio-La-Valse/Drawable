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
        /// The X-coorindate of the first point of the line.
        /// </summary>
        public double X1 { get; }
        public double X2 { get; }
        public double Y1 { get; }
        public double Y2 { get; }
        public double Thickness { get; }
        public XY TopLeft => new XY(Math.Min(X1, X2), Math.Min(Y1, Y2));
        public XY BottomRight => new XY(Math.Max(X1, X2), Math.Max(Y1, Y2));


        public DrawableLine(double x1, double x2, double y1, double y2, ColorARGB color, double thickness)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            Thickness = thickness;
            Color = color;
        }

        public DrawableLine(XY start, XY end, ColorARGB color, double thickness)
        {
            X1 = start.X;
            X2 = end.X;
            Y1 = start.Y;
            Y2 = end.Y;
            Thickness = thickness;
            Color = color;
        }

        public DrawableLine(Line line, double thickness, ColorARGB color)
        {
            X1 = line.Start.X;
            X2 = line.End.X;
            Y1 = line.Start.Y;
            Y2 = line.End.Y;
            Thickness = thickness;
            Color = color;
        }

        public DrawableTrapezoid ToTrapezoid() =>
            new DrawableTrapezoid(new XY(X1, Y1), new XY(X2, Y2), Thickness, Color);

        public override BoundingBox GetBoundingBox() =>
            new BoundingBox(X1 - Thickness / 2, X2 + Thickness / 2, Y1 - Thickness / 2, Y2 + Thickness / 2);

        public double GetDistance(XY point)
        {
            return new Line(X1, Y1, X2, Y2).Distance(point);
        }
    }
}
