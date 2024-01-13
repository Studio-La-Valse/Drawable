using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.HTML
{
    /// <summary>
    /// An experimental implementation of the <see cref="BaseBitmapPainter{TBitmap}"/> that draws drawable elements into a html/svg string.
    /// </summary>
    public class HTMLBitmapPainter : BaseLazyBitmapPainter<HTMLCanvas>
    {
        private readonly HTMLCanvas canvas;

        public HTMLBitmapPainter(HTMLCanvas canvas) : base(canvas)
        {
            this.canvas = canvas;
        }

        public override void DrawBackground(ColorARGB color)
        {
            canvas.Background = color;
        }

        public override void InitDrawing()
        {

        }

        protected override void DrawElement(HTMLCanvas canvas, DrawableLine line)
        {
            var fillr = line.Color.Red;
            var fillg = line.Color.Blue;
            var fillb = line.Color.Green;

            var _line =
                $"<line " +
                    $"x1=\"{line.X1}\" ".Replace(",", ".") +
                    $"y1=\"{line.Y1}\" ".Replace(",", ".") +
                    $"x2=\"{line.X2}\" ".Replace(",", ".") +
                    $"y2=\"{line.Y2}\" ".Replace(",", ".") +
                    $"style=\"" +
                        $"stroke:rgb({fillr},{fillg},{fillb}); " +
                        $"stroke-width:{line.Thickness}; ".Replace(",", ".") +
                        $"opacity:{line.Color.Alpha / 255D};\" ".Replace(",", ".") +
                    $"visibility=\"visible\"" +
                "/>";

            canvas.Add(_line);
        }

        protected override void DrawElement(HTMLCanvas canvas, DrawableRectangle rectangle)
        {
            var fillr = rectangle.Color.Red;
            var fillg = rectangle.Color.Blue;
            var fillb = rectangle.Color.Green;
            var filla = (rectangle.Color.Alpha / 255D).ToString().Replace(",", ".");

            var stroker = rectangle.StrokeColor?.Red ?? 0;
            var strokeg = rectangle.StrokeColor?.Green ?? 0;
            var strokeb = rectangle.StrokeColor?.Blue ?? 0;

            var rect =
                $"<rect " +
                    $"x=\"{rectangle.TopLeftX}\" ".Replace(",", ".") +
                    $"y=\"{rectangle.TopLeftY}\" ".Replace(",", ".") +
                    $"width=\"{rectangle.Width}\" ".Replace(",", ".") +
                    $"height=\"{rectangle.Height}\" ".Replace(",", ".") +
                    $"style=\"" +
                        $"fill:rgb({fillr},{fillg},{fillb}); " +
                        $"stroke-width:{rectangle.StrokeWeight}; ".Replace(",", ".") +
                        $"stroke:rgb({stroker},{strokeg},{strokeb}); " +
                        $"opacity:{filla};\" " +
                    $"visibility=\"visible\"" +
                "/>";

            canvas.Add(rect);
        }

        protected override void DrawElement(HTMLCanvas canvas, DrawableText text)
        {
            var x = $"{text.TopLeftX}".Replace(",", ".");

            var y = $"{text.BottomLeftY}".Replace(",", ".");

            var fontStyle = $"font-size=\"{text.FontSize}px\" font-family=\"{text.FontFamily.Name}\"".Replace(",", ".");

            var t =
                $"<text x=\"{x}\" y=\"{y}\" {fontStyle} visibility=\"visible\">" +
                    text.Text +
                "</text>";

            canvas.Add(t);
        }

        protected override void DrawElement(HTMLCanvas canvas, DrawableEllipse ellipse)
        {

        }

        protected override void DrawElement(HTMLCanvas canvas, DrawablePolyline polyline)
        {
            var svg = "<polyline points=\"";

            foreach (var point in polyline.Points)
            {
                var x = point.X.ToString().Replace(",", ".");
                var y = point.Y.ToString().Replace(",", ".");

                svg += $"{x},{y} ";
            }

            svg += "\" />";

            canvas.Add(svg);
        }

        protected override void DrawElement(HTMLCanvas canvas, DrawablePolygon polygon)
        {
            var svg = "<polygon points=\"";

            foreach (var point in polygon.Points)
            {
                var x = point.X.ToString().Replace(",", ".");
                var y = point.Y.ToString().Replace(",", ".");

                svg += $"{x},{y} ";
            }

            svg += "\" />";

            canvas.Add(svg);
        }

        protected override void DrawElement(HTMLCanvas canvas, DrawableBezierCurve bezier)
        {
            throw new NotImplementedException();
        }
    }
}
