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

        /// <summary>
        /// The color of the text.
        /// </summary>
        public ColorARGB Color { get; }
        /// <summary>
        /// The platform independent font family.
        /// </summary>
        public FontFamilyCore FontFamily { get; }
        /// <summary>
        /// The X-coordinate of the top left bounding box of the text. Note that this value will be smaller than the value of the BottomLeftX property.
        /// </summary>
        public double TopLeftX { get; }
        /// <summary>
        /// The X-coordinate of the top left bounding box of the text. Note that this value will be smaller than the value of the BottomLeftY property.
        /// </summary>
        public double TopLeftY { get; }
        /// <summary>
        /// The Y-coordinate of the bottom left point of the bounding box of the text.
        /// </summary>
        public virtual double BottomLeftY => TopLeftY + Dimensions.Y;
        /// <summary>
        /// The font size.
        /// </summary>
        public double FontSize { get; }
        /// <summary>
        /// The text to draw.
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// The dimensions of the boundingbox of the text.
        /// </summary>
        public XY Dimensions
        {
            get
            {
                return dimensions ??= ExternalTextMeasure.TextMeasurer.Measure(Text, FontFamily, FontSize);
            }
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="originX"></param>
        /// <param name="originY"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="color"></param>
        /// <param name="alignment"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="font"></param>
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

        /// <inheritdoc/>
        public override BoundingBox GetBoundingBox()
        {
            var dimensions = Dimensions;

            return new BoundingBox(TopLeftX, TopLeftX + dimensions.X, TopLeftY, TopLeftY + dimensions.Y);
        }
    }
}
