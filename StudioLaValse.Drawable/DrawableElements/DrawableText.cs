using StudioLaValse.Drawable.Text;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// Represents drawable text as an extension of the <see cref="BaseDrawableElement"/>. Requires <see cref="ExternalTextMeasure.TextMeasurer"/> to be set to a target specific implementation before the the constructor of any instance is called.
    /// </summary>
    public class DrawableText : BaseDrawableElement
    {
        private XY? dimensions;

        public ColorARGB Color { get; }
        public FontFamilyCore FontFamily { get; }
        public double TopLeftX { get; }
        public double TopLeftY { get; }
        public virtual double BottomLeftY => TopLeftY + Dimensions.Y;
        public double FontSize { get; }
        public string Text { get; }
        public XY Dimensions
        {
            get
            {
                return dimensions ??= ExternalTextMeasure.TextMeasurer.Measure(Text, FontFamily, FontSize);
            }
        }

        public DrawableText(
            double originX,
            double originY,
            string text,
            double fontSize,
            ColorARGB color,
            HorizontalTextOrigin alignment = HorizontalTextOrigin.Left,
            VerticalTextOrigin verticalAlignment = VerticalTextOrigin.Top,
            FontFamilyCore? font = null)
        {
            TopLeftX = originX;
            TopLeftY = originY;
            FontSize = fontSize;
            Text = text;
            Color = color;
            FontFamily = font ?? new FontFamilyCore("Arial");

            if (alignment != HorizontalTextOrigin.Left)
            {
                if (alignment == HorizontalTextOrigin.Center)
                    TopLeftX -= Dimensions.X / 2;
                else
                    TopLeftX -= Dimensions.X;
            }

            if (verticalAlignment != VerticalTextOrigin.Top)
            {
                if (verticalAlignment == VerticalTextOrigin.Center)
                    TopLeftY -= Dimensions.Y / 2;
                else
                    TopLeftY -= Dimensions.Y;
            }

        }

        public override BoundingBox GetBoundingBox()
        {
            var dimensions = Dimensions;

            return new BoundingBox(TopLeftX, TopLeftX + dimensions.X, TopLeftY, TopLeftY + dimensions.Y);
        }
    }
}
