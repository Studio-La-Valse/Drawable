using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.HTML.Extensions
{
    /// <summary>
    /// Extensions for drawable elements.
    /// </summary>
    public static class DrawableElementExtensions
    {
        /// <summary>
        /// Extract the color rgb string value.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Svg(this ColorARGB color)
        {
            var fillr = color.Red;
            var fillg = color.Blue;
            var fillb = color.Green;

            var rgb = $"rgb({fillr},{fillg},{fillb})";
            return rgb;
        }

        /// <summary>
        /// Extract the color rgb string value.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Svg(this ColorRGB color)
        {
            var fillr = color.Red;
            var fillg = color.Blue;
            var fillb = color.Green;

            var rgb = $"rgb({fillr},{fillg},{fillb})";
            return rgb;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string Svg(this DrawableLine line)
        {
            var style = $"stroke:{line.Color.Svg()}; " +
                        $"stroke-width:{line.Thickness}; ".Replace(",", ".") +
                        $"opacity:{line.Color.Alpha / 255D};".Replace(",", ".");

            var _line = $"<line " +
                        $"x1=\"{line.X1}\" ".Replace(",", ".") +
                        $"y1=\"{line.Y1}\" ".Replace(",", ".") +
                        $"x2=\"{line.X2}\" ".Replace(",", ".") +
                        $"y2=\"{line.Y2}\" ".Replace(",", ".") +
                        $"style=\"{style}\"" +
                        "/>";

            return _line;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static string Svg(this DrawableRectangle rectangle)
        {
            var filla = (rectangle.Color.Alpha / 255D).ToString().Replace(",", ".");

            var stroker = rectangle.StrokeColor?.Red ?? 0;
            var strokeg = rectangle.StrokeColor?.Green ?? 0;
            var strokeb = rectangle.StrokeColor?.Blue ?? 0;

            var style = $"fill:{rectangle.Color.Svg()}; " +
                        $"stroke-width:{rectangle.StrokeWeight}; ".Replace(",", ".") +
                        $"stroke:rgb({stroker},{strokeg},{strokeb}); " +
                        $"opacity:{filla};";

            var rect = $"<rect " +
                       $"x=\"{rectangle.TopLeftX}\" ".Replace(",", ".") +
                       $"y=\"{rectangle.TopLeftY}\" ".Replace(",", ".") +
                       $"width=\"{rectangle.Width}\" ".Replace(",", ".") +
                       $"height=\"{rectangle.Height}\" ".Replace(",", ".") +
                       $"style=\"{style}\"" +
                       "/>";

            return rect;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Svg(this DrawableText text)
        {
            var x = $"{text.OriginX}".Replace(",", ".");
            var y = $"{text.OriginY}".Replace(",", ".");

            var fontStyle = $"font-size=\"{text.FontSize}px\" " +
                            $"font-family=\"{text.FontFamily.Name}\" ".Replace(",", ".");

            var alignmentBase = text.VerticalAlignment switch
            {
                VerticalTextOrigin.Top => "hanging",
                VerticalTextOrigin.Center => "middle",
                VerticalTextOrigin.Bottom => "baseline",
                _ => throw new NotImplementedException(nameof(text.VerticalAlignment))
            };
            var textAnchor = text.HorizontalAlignment switch
            {
                HorizontalTextOrigin.Left => "left",
                HorizontalTextOrigin.Right => "right",
                HorizontalTextOrigin.Center => "middle",
                _ => throw new NotImplementedException(nameof(text.HorizontalAlignment))
            };
            var t = $"<text alignment-baseline=\"{alignmentBase}\" text-anchor=\"{textAnchor}\" x=\"{x}\" y=\"{y}\" {fontStyle}>{text.Text}</text>";

            return t;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <returns></returns>
        public static string Svg(this DrawableEllipse ellipse)
        {
            var cx = ellipse.CenterX;
            var cy = ellipse.CenterY;
            var rx = ellipse.Width / 2;
            var ry = ellipse.Height / 2;

            var filla = (ellipse.Color.Alpha / 255D).ToString().Replace(",", ".");

            var stroker = ellipse.StrokeColor?.Red ?? 0;
            var strokeg = ellipse.StrokeColor?.Green ?? 0;
            var strokeb = ellipse.StrokeColor?.Blue ?? 0;

            var style = $"fill:{ellipse.Color.Svg()}; " +
                        $"stroke-width:{ellipse.StrokeWeight}; ".Replace(",", ".") +
                        $"stroke:rgb({stroker},{strokeg},{strokeb}); " +
                        $"opacity:{filla};";

            var svg = $"<ellipse cx=\"{cx}\" cy=\"{cy}\" rx=\"{rx}\" ry=\"{ry}\" style=\"{style}\"/>";
            return svg;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static string Svg(this DrawablePolyline polyline)
        {
            var fillr = polyline.Color.Red;
            var fillg = polyline.Color.Blue;
            var fillb = polyline.Color.Green;
            var filla = (polyline.Color.Alpha / 255D).ToString().Replace(",", ".");

            var stroker = polyline.Color?.Red ?? 0;
            var strokeg = polyline.Color?.Green ?? 0;
            var strokeb = polyline.Color?.Blue ?? 0;

            var style = $"stroke-width:{polyline.StrokeWeight}; ".Replace(",", ".") +
                        $"stroke:rgb({stroker},{strokeg},{strokeb}); " +
                        $"opacity:{filla}; ";

            var svg = $"<polyline style=\"{style}\" points=\"";

            foreach (var point in polyline.Points)
            {
                var x = point.X.ToString().Replace(",", ".");
                var y = point.Y.ToString().Replace(",", ".");

                svg += $"{x},{y} ";
            }

            svg += "\" />";

            return svg;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static string Svg(this DrawablePolygon polygon)
        {
            var fillr = polygon.Fill?.Red ?? 0;
            var fillg = polygon.Fill?.Blue ?? 0;
            var fillb = polygon.Fill?.Green ?? 0;
            var filla = polygon.Fill is not null ?
                (polygon.Fill.Alpha / 255D).ToString().Replace(",", ".") :
                "0";

            var stroker = polygon.Color?.Red ?? 0;
            var strokeg = polygon.Color?.Green ?? 0;
            var strokeb = polygon.Color?.Blue ?? 0;
            var strokea = polygon.Color?.Alpha ?? 0;


            var style = strokea == 0 ?
                $"fill:rgb({fillr},{fillg},{fillb}); " +
                $"stroke-width:0; " +
                $"opacity:{filla}; " :

                $"fill:rgb({fillr},{fillg},{fillb}); " +
                $"stroke-width:{polygon.StrokeWeight}; ".Replace(",", ".") +
                $"stroke:rgb({stroker},{strokeg},{strokeb}); " +
                $"opacity:{filla}; ";

            var svg = $"<polygon style=\"{style}\" points=\"";

            foreach (var point in polygon.Points)
            {
                var x = point.X.ToString().Replace(",", ".");
                var y = point.Y.ToString().Replace(",", ".");

                svg += $"{x},{y} ";
            }

            svg += "\" />";

            return svg;
        }

        /// <summary>
        /// Transform the element to an svg string.
        /// </summary>
        /// <param name="bezier"></param>
        /// <returns></returns>
        public static string Svg(this DrawableBezierCurve bezier)
        {
            throw new NotImplementedException();
        }
    }
}