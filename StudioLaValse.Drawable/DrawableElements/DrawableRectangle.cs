using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents a drawable rectangle as an extension of the <see cref="BaseDrawableElement"/>
    /// </summary>
    public sealed class DrawableRectangle : BaseDrawableElement
    {
        /// <summary>
        /// The x coordinate of the top left point of the rectangle.
        /// </summary>
        public double TopLeftX { get; }
        /// <summary>
        /// The y coordinate of the top left point of the rectangle.
        /// </summary>
        public double TopLeftY { get; }
        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public double Height { get; }
        /// <summary>
        /// The radius of the corner rounding of the rectangle.
        /// </summary>
        public double CornerRadius { get; }
        /// <summary>
        /// The stroke thickness.
        /// </summary>
        public double StrokeWeight { get; }
        /// <summary>
        /// The fill color.
        /// </summary>
        public ColorARGB Color { get; }
        /// <summary>
        /// The stroke color.
        /// </summary>
        public ColorARGB? StrokeColor { get; }

        /// <summary>
        /// Construct a drawable rectangle from coordinates.
        /// </summary>
        /// <param name="topLeftX"></param>
        /// <param name="topLeftY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <param name="strokeWeight"></param>
        /// <param name="strokeColor"></param>
        /// <param name="cornerRounding"></param>
        public DrawableRectangle(double topLeftX, double topLeftY, double width, double height, ColorARGB color, double strokeWeight = 0, ColorARGB? strokeColor = null, double cornerRounding = 0)
        {
            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            Width = width;
            Height = height;
            Color = color;
            StrokeColor = strokeColor;
            CornerRadius = cornerRounding;
            StrokeWeight = strokeWeight;
        }

        /// <summary>
        /// Construct a drawable rectangle from a bounding box.
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <param name="color"></param>
        /// <param name="strokeWeight"></param>
        /// <param name="strokeColor"></param>
        /// <param name="cornerRounding"></param>
        public DrawableRectangle(BoundingBox boundingBox, ColorARGB color, double strokeWeight = 0, ColorARGB? strokeColor = null, double cornerRounding = 0)
        {
            TopLeftX = boundingBox.MinPoint.X;
            TopLeftY = boundingBox.MinPoint.Y;
            Width = boundingBox.Width;
            Height = boundingBox.Height;
            Color = color;
            StrokeColor = strokeColor;
            CornerRadius = cornerRounding;
            StrokeWeight = strokeWeight;
        }

        /// <inheritdoc/>
        public override BoundingBox GetBoundingBox() =>
            new (TopLeftX, TopLeftX + Width, TopLeftY, TopLeftY + Height);

        /// <inheritdoc/>
        public override XY ClosestPointEdge(XY other)
        {
            return GetBoundingBox().ClosestPointEdge(other);
        }

        /// <inheritdoc/>
        public override XY ClosestPointShape(XY other)
        {
            return GetBoundingBox().ClosestPointShape(other);
        }
    }
}
