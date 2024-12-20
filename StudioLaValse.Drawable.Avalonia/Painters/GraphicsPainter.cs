using Avalonia.Media;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.Avalonia.Extensions;
using Avalonia;
using Avalonia.Controls.Shapes;
using StudioLaValse.Drawable.Text;

namespace StudioLaValse.Drawable.Avalonia.Painters;

/// <inheritdoc/>
public class GraphicsPainter : BaseCachingBitmapPainter<DrawingContext>
{
    private readonly InteractiveControl drawingContext;
    private readonly IMeasureText textMeasurer;

    /// <inheritdoc/>
    protected override List<Action<DrawingContext>> Cache => drawingContext.DrawActions;

    /// <inheritdoc/>
    public GraphicsPainter(InteractiveControl drawingContext, IMeasureText textMeasurer)
    {
        this.drawingContext = drawingContext;
        this.textMeasurer = textMeasurer;
    }

    /// <inheritdoc/>
    public override void DrawBackground(ColorARGB colorARGB)
    {

    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableLine line)
    {
        var pen = line.Color.ToPen(line.Thickness);
        var firstPoint = new Point(line.X1, line.Y1);
        var secondPoint = new Point(line.X2, line.Y2);
        drawingContext.DrawLine(pen, firstPoint, secondPoint);
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableRectangle rectangle)
    {
        var rect = new Rect(new Point((int)rectangle.TopLeftX, (int)rectangle.TopLeftY), new Size((int)rectangle.Width, (int)rectangle.Height));
        drawingContext.FillRectangle(rectangle.Color.ToBrush(), rect);

        if (rectangle.StrokeColor != null && rectangle.StrokeWeight > 0)
        {
            var pen = rectangle.StrokeColor.Value.ToPen(rectangle.StrokeWeight);
            drawingContext.DrawRectangle(pen, rect);
        }
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableText text)
    {
        drawingContext.DrawText(text.ToFormattedText(), new Point(text.GetLeft(textMeasurer), text.GetTop(textMeasurer)));
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableEllipse ellipse)
    {
        var brush = ellipse.Color.ToBrush();
        var x = ellipse.CenterX - ellipse.Width / 2;
        var y = ellipse.CenterY - ellipse.Height / 2;
        var width = ellipse.Width;
        var height = ellipse.Height;
        var rect = new Rect(x, y, width, height);
        if (ellipse.StrokeColor != null && ellipse.StrokeWeight > 0)
        {
            var strokeBrush = ellipse.StrokeColor.Value.ToBrush();
            var pen = new Pen(strokeBrush, (float)ellipse.StrokeWeight);
            drawingContext.DrawEllipse(brush, pen, rect);
        }
        else
        {
            drawingContext.DrawEllipse(brush, null, rect);
        }
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawablePolyline polyline)
    {
        if (!polyline.Points.Any())
        {
            return;
        }

        var segments = new PathSegments()
        {
            new PolyLineSegment(polyline.Points.Skip(1).Select(p => p.ToPoint()))
        };

        var figures = new PathFigures()
        {
            new PathFigure()
            {
                StartPoint = polyline.Points.First().ToPoint(),
                Segments = segments,
                IsFilled = false
            }
        };

        var geometry = new PathGeometry()
        {
            Figures = figures
        };

        var brush = polyline.Color.ToBrush();

        drawingContext.DrawGeometry(null, new Pen(brush, polyline.StrokeWeight), geometry);
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawablePolygon polygon)
    {
        var segments = new PathSegments();
        var isStroked = polygon.Color != null && polygon.StrokeWeight > 0;

        foreach (var point in polygon.Points.Skip(1))
        {
            var segment = new LineSegment()
            {
                Point = point.ToPoint()
            };

            segments.Add(segment);
        }

        var figures = new PathFigures()
        {
            new PathFigure()
            {
                StartPoint = polygon.Points.First().ToPoint(),
                IsFilled = true,
                IsClosed = true,
                Segments = segments,
            }
        };

        var geometry = new PathGeometry()
        {
            Figures = figures
        };

        var pen = isStroked ?
            new Pen(polygon.Color!.Value.ToBrush(), polygon.StrokeWeight) : null;

        drawingContext.DrawGeometry(polygon.Fill?.ToBrush(), pen, geometry);
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext canvas, DrawableBezierQuadratic bezier)
    {
        var segments = new PathSegments();
        var segment = new global::Avalonia.Media.QuadraticBezierSegment()
        {
            Point1 = bezier.Second.ToPoint(),
            Point2 = bezier.Third.ToPoint(),
        };
        segments.Add(segment);

        var pathFigures = new PathFigures()
        {
            new PathFigure()
            {
                StartPoint = bezier.First.ToPoint(),
                Segments = segments,
                IsFilled = false,
                IsClosed = false
            }
        };
        var geometry = new PathGeometry()
        {
            Figures = pathFigures,
        };

        var pen = new Pen(bezier.Color.ToBrush(), bezier.StrokeWeight);
        canvas.DrawGeometry(null, pen, geometry);
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext canvas, DrawableBezierCubic bezier)
    {
        var segments = new PathSegments();
        var segment = new BezierSegment()
        {
            Point1 = bezier.Second.ToPoint(),
            Point2 = bezier.Third.ToPoint(),
            Point3 = bezier.Fourth.ToPoint(),
        };
        segments.Add(segment);

        var pathFigures = new PathFigures()
        {
            new PathFigure()
            {
                StartPoint = bezier.First.ToPoint(),
                Segments = segments,
                IsFilled = false,
                IsClosed = false
            }
        };
        var geometry = new PathGeometry()
        {
            Figures = pathFigures,
        };

        var pen = new Pen(bezier.Color.ToBrush(), bezier.StrokeWeight);
        canvas.DrawGeometry(null, pen, geometry);
    }

    /// <inheritdoc/>
    public override void FinishDrawing()
    {
        drawingContext.InvalidateVisual();
    }
}
