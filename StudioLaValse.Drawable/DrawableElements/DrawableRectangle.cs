using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents a drawable rectangle as an extension of the <see cref="BaseDrawableElement"/>
    /// </summary>
    public sealed class DrawableRectangle : BaseDrawableElement
    {
        public double TopLeftX { get; }
        public double TopLeftY { get; }
        public double Width { get; }
        public double Height { get; }
        public double CornerRadius { get; }
        public double StrokeWeight { get; }
        public ColorARGB Color { get; }
        public ColorARGB? StrokeColor { get; }

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

        public override BoundingBox GetBoundingBox() =>
            new BoundingBox(TopLeftX, TopLeftX + Width, TopLeftY, TopLeftY + Height);
    }
}
