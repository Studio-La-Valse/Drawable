﻿using Avalonia.Media;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.Avalonia.Extensions;
using Avalonia;
using Avalonia.Controls.Shapes;

namespace StudioLaValse.Drawable.Avalonia.Painters;

public class GraphicsPainter : BaseCachingBitmapPainter<DrawingContext>
{
    private readonly InteractiveControl drawingContext;

    protected override List<Action<DrawingContext>> Cache => drawingContext.DrawActions;

    public GraphicsPainter(InteractiveControl drawingContext)
    {
        this.drawingContext = drawingContext;
    }


    public override void DrawBackground(ColorARGB colorARGB)
    {

    }

    protected override void DrawElement(DrawingContext drawingContext, DrawableLine line)
    {
        var pen = line.Color.ToPen(line.Thickness);
        drawingContext.DrawLine(pen, line.TopLeft.ToPoint(), line.BottomRight.ToPoint());
    }

    protected override void DrawElement(DrawingContext drawingContext, DrawableRectangle rectangle)
    {
        var rect = new Rect(new Point((int)rectangle.TopLeftX, (int)rectangle.TopLeftY), new Size((int)rectangle.Width, (int)rectangle.Height));
        drawingContext.FillRectangle(rectangle.Color.ToBrush(), rect);

        if (rectangle.StrokeColor != null && rectangle.StrokeWeight > 0)
        {
            var pen = rectangle.StrokeColor.ToPen(rectangle.StrokeWeight);
            drawingContext.DrawRectangle(pen, rect);
        }
    }

    protected override void DrawElement(DrawingContext drawingContext, DrawableText text)
    {
        drawingContext.DrawText(text.ToFormattedText(), new Point(text.TopLeftX, text.BottomLeftY));
    }

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
            var strokeBrush = ellipse.StrokeColor.ToBrush();
            var pen = new Pen(strokeBrush, (float)ellipse.StrokeWeight);
            drawingContext.DrawEllipse(brush, pen, rect);
        }
        else
        {
            drawingContext.DrawEllipse(brush, null, rect);
        }
    }

    protected override void DrawElement(DrawingContext drawingContext, DrawablePolyline polyline)
    {
        var strokeBrush = polyline.Color?.ToBrush() ?? new SolidColorBrush();
        var pen = new Pen(strokeBrush, polyline.StrokeWeight);
        var geometry = new PolylineGeometry()
        {
            Points = polyline.Points.Select(p => p.ToPoint()).ToArray(),
        };
        drawingContext.DrawGeometry(null, pen, geometry);
    }

    protected override void DrawElement(DrawingContext drawingContext, DrawablePolygon polygon)
    {
        var fillBrush = polygon.Fill?.ToBrush() ?? new SolidColorBrush();
        var strokeBrush = polygon.Color?.ToBrush() ?? new SolidColorBrush();
        var pen = new Pen(strokeBrush, polygon.StrokeWeight);
        var geometry = new PolylineGeometry()
        {
            Points = polygon.Points.Select(p => p.ToPoint()).ToArray(),
        };
        drawingContext.DrawGeometry(fillBrush, pen, geometry);
    }

    protected override void DrawElement(DrawingContext canvas, DrawableBezierCurve bezier)
    {
        throw new NotImplementedException();
    }

    public override void FinishDrawing()
    {
        drawingContext.InvalidateVisual();
    }
}
