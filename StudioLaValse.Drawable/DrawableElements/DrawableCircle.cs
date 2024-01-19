using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// A drawable circle as an extension of a drawable ellipse.
    /// </summary>
    public sealed class DrawableCircle : DrawableEllipse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="color"></param>
        /// <param name="stroke"></param>
        /// <param name="strokeWeight"></param>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="stroke"></param>
        /// <param name="strokeWeight"></param>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="stroke"></param>
        /// <param name="strokeWeight"></param>
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
    }
}
