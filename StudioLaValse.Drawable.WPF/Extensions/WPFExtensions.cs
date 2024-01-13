using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StudioLaValse.Drawable.WPF.Extensions
{
    public static class WPFExtensions
    {
        private static readonly SolidColorBrush Black = Brushes.Black;
        private static readonly SolidColorBrush White = Brushes.White;


        public static SolidColorBrush ToWindowsBrush(this ColorRGB? colorRGB)
        {
            if (colorRGB is null)
            {
                return Black;
            }

            var argb = new ColorARGB(ColorRGB.MaxValue, colorRGB);

            return argb.ToWindowsBrush();
        }
        public static SolidColorBrush ToWindowsBrush(this ColorARGB? colorARGB)
        {
            if (colorARGB is null)
            {
                return Black;
            }

            if (!colorARGB.IsDifferent(ColorARGB.Black))
            {
                return Black;
            }

            if (!colorARGB.IsDifferent(ColorARGB.White))
            {
                return White;
            }

            var windowsColor = Color.FromArgb(
                Convert.ToByte(colorARGB.Alpha),
                Convert.ToByte(colorARGB.Red),
                Convert.ToByte(colorARGB.Green),
                Convert.ToByte(colorARGB.Blue));

            return new SolidColorBrush(windowsColor);
        }


        public static Point ToWindowsPoint(this XY p)
        {
            return new Point(p.X, p.Y);
        }


        public static FormattedText AsFormattedText(this string text, FontFamily fontFamily, double fontSize)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily, new FontStyle(), new FontWeight(), new FontStretch()),
                fontSize,
                Brushes.Black,
                1);

            return formattedText;
        }
        public static FormattedText AsFormattedText(this DrawableText text)
        {
            var formattedText = new FormattedText(
                text.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(text.FontFamily.Name), new FontStyle(), new FontWeight(), new FontStretch()),
                text.FontSize,
                text.Color.ToWindowsBrush(),
                1);

            return formattedText;
        }
        public static XY MeasureTextSize(this FormattedText formattedText)
        {
            return new XY(formattedText.Width, formattedText.Height);
        }





        public static UIElement ToUIElement(this DrawableEllipse ellipse)
        {
            var ellipseShape = new Ellipse()
            {
                Width = ellipse.Width,
                Height = ellipse.Height,
                Fill = ellipse.Color.ToWindowsBrush(),
            };

            if (ellipse.StrokeColor != null && ellipse.StrokeWeight > 0)
            {
                ellipseShape.Stroke = ellipse.StrokeColor.ToWindowsBrush();
                ellipseShape.StrokeThickness = ellipse.StrokeWeight;
            }

            return ellipseShape;
        }
        public static UIElement ToUIElement(this DrawableLine line)
        {
            var lineShape = new System.Windows.Shapes.Line()
            {
                X1 = line.X1,
                X2 = line.X2,
                Y1 = line.Y1,
                Y2 = line.Y2,
            };


            lineShape.Stroke = line.Color.ToWindowsBrush();
            lineShape.StrokeThickness = line.Thickness;

            return lineShape;
        }
        public static UIElement ToUIElement(this DrawableRectangle rectangle)
        {
            var rect = new Rectangle()
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Fill = rectangle.Color.ToWindowsBrush(),
                Stroke = rectangle.StrokeColor.ToWindowsBrush(),
                StrokeThickness = rectangle.StrokeWeight,
                RadiusX = rectangle.CornerRadius,
                RadiusY = rectangle.CornerRadius
            };

            return rect;
        }
        public static UIElement ToUIElement(this DrawableText text)
        {
            var textShape = new TextBlock()
            {
                FontFamily = new FontFamily(text.FontFamily.Name),
                FontSize = text.FontSize,
                Text = text.Text,
                Foreground = text.Color.ToWindowsBrush()
            };

            return textShape;
        }
        public static UIElement ToUIElement(this DrawablePolyline polyline)
        {
            var lineShape = new Polyline
            {
                Points = new PointCollection(polyline.Points.Select(p => new Point(p.X, p.Y))),
                Stroke = polyline.Color.ToWindowsBrush(),
                StrokeThickness = polyline.StrokeWeight
            };

            return lineShape;
        }
        public static UIElement ToUIElement(this DrawablePolygon polygon)
        {
            var lineShape = new Polygon
            {
                Points = new PointCollection(polygon.Points.Select(p => new Point(p.X, p.Y))),
                Fill = polygon.Fill.ToWindowsBrush(),
                Stroke = polygon.Color.ToWindowsBrush(),
                StrokeThickness = polygon.StrokeWeight
            };

            return lineShape;
        }
    }
}
