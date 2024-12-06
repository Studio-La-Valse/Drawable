using StudioLaValse.Drawable.Text;
using StudioLaValse.Geometry;
using System.Diagnostics.CodeAnalysis;

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
        /// The horizontal text alignment: <see cref="HorizontalTextOrigin.Left"/>, <see cref="HorizontalTextOrigin.Center"/>, <see cref="HorizontalTextOrigin.Right"/>.
        /// </summary>
        public HorizontalTextOrigin HorizontalAlignment { get; }
        /// <summary>
        /// The X-coordinate of the origin of the text. Note the corresponding horizontal alignment.
        /// </summary>
        public double OriginX { get; }
        /// <summary>
        /// The vertical text alignment: <see cref="VerticalTextOrigin.Top"/>, <see cref="VerticalTextOrigin.Center"/>, <see cref="VerticalTextOrigin.Bottom"/>.
        /// </summary>
        public VerticalTextOrigin VerticalAlignment { get; }
        /// <summary>
        /// The Y-coordinate of the origin of the text. Note the corresponding vertical alignment.
        /// </summary>
        public double OriginY { get; }
        /// <summary>
        /// The font size.
        /// </summary>
        public double FontSize { get; }
        /// <summary>
        /// The text to draw.
        /// </summary>
        public string Text { get; }


        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="originX"></param>
        /// <param name="originY"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="color"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="fontFamily"></param>
        public DrawableText(
            double originX,
            double originY,
            string text,
            double fontSize,
            ColorARGB color,
            HorizontalTextOrigin horizontalAlignment,
            VerticalTextOrigin verticalAlignment,
            FontFamilyCore fontFamily)
        {
            OriginX = originX;
            OriginY = originY;
            Text = text;
            FontSize = fontSize;
            Color = color;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            FontFamily = fontFamily;
        }

        /// <summary>
        /// Get the left coordinate of the text. Text measurement is skipped if text aligns left.
        /// </summary>
        /// <param name="measureText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double GetLeft(IMeasureText measureText)
        {
            if(HorizontalAlignment == HorizontalTextOrigin.Left)
            {
                return OriginX;
            }

            this.dimensions ??= measureText.Measure(Text, FontFamily, FontSize);
            if (HorizontalAlignment == HorizontalTextOrigin.Center)
            {
                var left = OriginX - dimensions.Value.X / 2;
                return left;
            }

            if (HorizontalAlignment == HorizontalTextOrigin.Right)
            {
                var left = OriginX - dimensions.Value.X;
                return left;
            }

            throw new NotImplementedException(nameof(HorizontalAlignment));
        }

        /// <summary>
        /// Get the right coordinate of the text. Text measurement is skipped if text aligns right.
        /// </summary>
        /// <param name="measureText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double GetRight(IMeasureText measureText)
        {
            if (HorizontalAlignment == HorizontalTextOrigin.Right)
            {
                return OriginX;
            }

            this.dimensions ??= measureText.Measure(Text, FontFamily, FontSize);
            if (HorizontalAlignment == HorizontalTextOrigin.Center)
            {
                var right = OriginX + dimensions.Value.X / 2;
                return right;
            }

            if (HorizontalAlignment == HorizontalTextOrigin.Left)
            {
                var right = OriginX + dimensions.Value.X;
                return right;
            }

            throw new NotImplementedException(nameof(HorizontalAlignment));
        }

        /// <summary>
        /// Get the top coordinate of the text. Text measurement is skipped if text aligns at the top.
        /// </summary>
        /// <param name="measureText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double GetTop(IMeasureText measureText)
        {
            if (VerticalAlignment == VerticalTextOrigin.Top)
            {
                return OriginY;
            }

            this.dimensions ??= measureText.Measure(Text, FontFamily, FontSize);
            if (VerticalAlignment == VerticalTextOrigin.Center)
            {
                var top = OriginY - dimensions.Value.Y / 2;
                return top;
            }

            if (VerticalAlignment == VerticalTextOrigin.Bottom)
            {
                var top = OriginY - dimensions.Value.Y;
                return top;
            }

            throw new NotImplementedException(nameof(HorizontalAlignment));
        }

        /// <summary>
        /// Get the bottom coordinate of the text. Text measurement is skipped if text aligns at the bottom.
        /// </summary>
        /// <param name="measureText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double GetBottom(IMeasureText measureText)
        {
            if (VerticalAlignment == VerticalTextOrigin.Bottom)
            {
                return OriginY;
            }

            this.dimensions ??= measureText.Measure(Text, FontFamily, FontSize);
            if (VerticalAlignment == VerticalTextOrigin.Center)
            {
                var bottom = OriginY + dimensions.Value.Y / 2;
                return bottom;
            }

            if (VerticalAlignment == VerticalTextOrigin.Top)
            {
                var bottom = OriginY + dimensions.Value.Y;
                return bottom;
            }

            throw new NotImplementedException(nameof(HorizontalAlignment));
        }


        /// <inheritdoc/>
        public override BoundingBox GetBoundingBox()
        {
            var dimensions = this.dimensions ??= ExternalTextMeasure.TextMeasurer.Measure(Text, FontFamily, FontSize);
            var topleftX = OriginX;
            var topleftY = OriginY;
            
            if (HorizontalAlignment != HorizontalTextOrigin.Left)
            {
                if (HorizontalAlignment == HorizontalTextOrigin.Center)
                {
                    topleftX -= dimensions.X / 2;
                }
                else
                {
                    topleftX -= dimensions.X;
                }
            }

            if (VerticalAlignment != VerticalTextOrigin.Top)
            {
                if (VerticalAlignment == VerticalTextOrigin.Center)
                {
                    topleftY -= dimensions.Y / 2;
                }
                else
                {
                    topleftY -= dimensions.Y;
                }
            }

            return new BoundingBox(topleftX, topleftX + dimensions.X, topleftY, topleftY + dimensions.Y);
        }

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
