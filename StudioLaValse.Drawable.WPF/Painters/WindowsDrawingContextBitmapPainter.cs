using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.Extensions;
using StudioLaValse.Drawable.WPF.Text;
using StudioLaValse.Drawable.WPF.UserControls;
using StudioLaValse.Geometry;
using System.Windows;
using System.Windows.Media;

namespace StudioLaValse.Drawable.WPF.Painters
{
    /// <summary>
    /// The default implementation for the <see cref="DrawingContext"/> bitmap.
    /// </summary>
    public class WindowsDrawingContextBitmapPainter : BaseCachingBitmapPainter<DrawingContext>
    {
        private readonly WindowsDrawingContextUserControl userControl;

        protected override List<Action<DrawingContext>> Cache => userControl.Cache;

        public WindowsDrawingContextBitmapPainter(WindowsDrawingContextUserControl userControl)
        {
            this.userControl = userControl;
            ExternalTextMeasure.TextMeasurer = new WPFTextMeasurer();
        }



        public override void DrawBackground(ColorARGB colorARGB)
        {
            userControl.Background = colorARGB.ToWindowsBrush();
        }

        protected override void DrawElement(DrawingContext drawingContext, DrawableLine line)
        {
            var pen = new Pen(line.Color.ToWindowsBrush(), line.Thickness);

            drawingContext.DrawLine(pen, new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
        }

        protected override void DrawElement(DrawingContext drawingContext, DrawableRectangle rectangle)
        {
            var rect = new Rect(new Point(rectangle.TopLeftX, rectangle.TopLeftY), new Size(rectangle.Width, rectangle.Height));

            var pen = rectangle.StrokeColor != null && rectangle.StrokeWeight > 0 ?
                new Pen(rectangle.StrokeColor.ToWindowsBrush(), rectangle.StrokeWeight) :
                null;

            drawingContext.DrawRectangle(rectangle.Color.ToWindowsBrush(), pen, rect);
        }

        protected override void DrawElement(DrawingContext drawingContext, DrawableText text)
        {
            var formattedText = text.AsFormattedText();

            drawingContext.DrawText(formattedText, new Point(text.TopLeftX, text.TopLeftY));
        }

        protected override void DrawElement(DrawingContext drawingContext, DrawableEllipse ellipse)
        {
            var pen = ellipse.StrokeColor != null && ellipse.StrokeWeight > 0 ?
                new Pen(ellipse.StrokeColor.ToWindowsBrush(), ellipse.StrokeWeight) :
                null;

            drawingContext.DrawEllipse(ellipse.Color.ToWindowsBrush(), pen, new Point(ellipse.CenterX, ellipse.CenterY), ellipse.Width / 2, ellipse.Height / 2);
        }

        protected override void DrawElement(DrawingContext drawingContext, DrawablePolyline polyline)
        {
            if (!polyline.Points.Any())
            {
                return;
            }

            var isStroked = polyline.Color != null && polyline.StrokeWeight > 0;

            var segments = new PathSegmentCollection()
            {
                new PolyLineSegment(polyline.Points.Skip(1).Select(p => p.ToWindowsPoint()), isStroked)
            };

            var figures = new List<PathFigure>()
            {
                new PathFigure(polyline.Points.First().ToWindowsPoint(), segments, false)
            };

            var geometry = new PathGeometry(figures);

            var brush = polyline.Color.ToWindowsBrush();

            drawingContext.DrawGeometry(null, new Pen(brush, polyline.StrokeWeight), geometry);
        }

        protected override void DrawElement(DrawingContext drawingContext, DrawablePolygon polygon)
        {
            var segments = new PathSegmentCollection();
            var isStroked = polygon.Color != null && polygon.StrokeWeight > 0;

            foreach (var point in polygon.Points.Skip(1))
            {
                var segment = new LineSegment(point.ToWindowsPoint(), isStroked);

                segments.Add(segment);
            }

            var figures = new List<PathFigure>()
            {
                new PathFigure(polygon.Points.First().ToWindowsPoint(), segments, true)
            };

            var geometry = new PathGeometry(figures);

            var pen = isStroked ?
                new Pen(polygon.Color.ToWindowsBrush(), polygon.StrokeWeight) : null;

            drawingContext.DrawGeometry(polygon.Fill.ToWindowsBrush(), pen, geometry);
        }

        protected override void DrawElement(DrawingContext canvas, DrawableBezierCurve bezier)
        {
            var enumerated = bezier.Points.ToList();
            if (enumerated.Count < 2)
            {
                return;
            }

            if (enumerated.Count == 2)
            {
                var _pen = new Pen(bezier.Color.ToWindowsBrush(), bezier.StrokeWeight);
                canvas.DrawLine(_pen, enumerated[0].ToWindowsPoint(), enumerated[1].ToWindowsPoint());
                return;
            }

            var segments = new PathSegmentCollection();
            var segment = new PolyBezierSegment(bezier.Points.Skip(1).Select(p => p.ToWindowsPoint()), true);
            segments.Add(segment);

            var pathFigures = new List<PathFigure>()
            {
                new PathFigure(enumerated[0].ToWindowsPoint(), segments, false)
            };
            var geometry = new PathGeometry(pathFigures);

            var pen = new Pen(bezier.Color.ToWindowsBrush(), bezier.StrokeWeight);
            canvas.DrawGeometry(null, pen, geometry);
        }


        public override void FinishDrawing()
        {
            userControl.Refresh();
        }
    }
}
