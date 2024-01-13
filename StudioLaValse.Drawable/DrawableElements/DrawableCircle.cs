using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// A drawable circle as an extension of a drawable ellipse.
    /// </summary>
    public sealed class DrawableCircle : DrawableEllipse
    {
        public DrawableCircle(
            Circle circle,
            ColorARGB color,
            ColorARGB? stroke = null,
            double strokeWeight = 0) : base(
                circle.Center.X,
                circle.Center.Y,
                circle.Radius * 2,
                circle.Radius * 2,
                color,
                stroke,
                strokeWeight)
        {

        }

        public DrawableCircle(
            double centerX,
            double centerY,
            double radius,
            ColorARGB color,
            ColorARGB? stroke = null,
            double strokeWeight = 0) : base(
                centerX,
                centerY,
                radius * 2,
                radius * 2,
                color,
                stroke,
                strokeWeight)
        {

        }

        public DrawableCircle(
            XY center,
            double radius,
            ColorARGB color,
            ColorARGB? stroke = null,
            double strokeWeight = 0) : base(
                center.X,
                center.Y,
                radius * 2,
                radius * 2,
                color,
                stroke,
                strokeWeight)
        {

        }

        public override BoundingBox GetBoundingBox() => base.GetBoundingBox();
    }
}
