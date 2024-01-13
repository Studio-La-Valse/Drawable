using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents a vertical trapezoid as an extension of the <see cref="DrawablePolygon"/>.
    /// </summary>
    public sealed class DrawableTrapezoid : DrawablePolygon
    {
        public DrawableTrapezoid(XY bottomLeft, XY bottomRight, double thicknessUp, ColorARGB fill) :
            base(ToPoints(bottomLeft, bottomRight, thicknessUp), fill, null, 0)
        {

        }

        public DrawableTrapezoid(Line line, double thicknessUp, ColorARGB fill) :
            base(ToPoints(line.Start, line.End, thicknessUp), fill, null, 0)
        {

        }

        public DrawableTrapezoid(XY bottomLeft, double length, double angle, double thicknessUp) : base(ToPoints(bottomLeft, bottomLeft.Move(length, angle), thicknessUp))
        {

        }




        private static IList<XY> ToPoints(XY bottomLeft, XY bottomRight, double thicknessUp)
        {
            return new List<XY>()
            {
                bottomLeft, bottomRight, bottomRight + new XY(0, thicknessUp), bottomLeft + new XY(0, thicknessUp)
            };
        }
    }
}
