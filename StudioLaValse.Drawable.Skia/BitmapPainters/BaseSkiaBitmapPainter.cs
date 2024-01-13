using SkiaSharp;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Skia.Extensions;

namespace StudioLaValse.Drawable.Skia.BitmapPainters
{
    /// <summary>
    /// The default implementation for all Skia bitmap painters.
    /// </summary>
    public abstract class BaseSkiaBitmapPainter : BaseCachingBitmapPainter<SKCanvas>
    {
        private readonly SKPath path = new();
        private readonly bool antiAlias = true;



        protected override void DrawElement(SKCanvas canvas, DrawableLine line)
        {
            var color = new SKColor((byte)line.Color.Red, (byte)line.Color.Green, (byte)line.Color.Blue, (byte)line.Color.Alpha);

            using var paint = new SKPaint() { Color = color, StrokeWidth = (float)line.Thickness, IsAntialias = true };
            paint.IsAntialias = antiAlias;

            canvas.DrawLine((float)line.X1, (float)line.Y1, (float)line.X2, (float)line.Y2, paint);
        }

        protected override void DrawElement(SKCanvas canvas, DrawableRectangle rectangle)
        {
            var color = new SKColor((byte)rectangle.Color.Red, (byte)rectangle.Color.Green, (byte)rectangle.Color.Blue, (byte)rectangle.Color.Alpha);

            using var paint = new SKPaint() { Color = color };
            paint.IsAntialias = antiAlias;

            canvas.DrawRect((float)rectangle.TopLeftX, (float)rectangle.TopLeftY, (float)rectangle.Width, (float)rectangle.Height, paint);
        }

        protected override void DrawElement(SKCanvas canvas, DrawableText text)
        {
            var color = new SKColor((byte)text.Color.Red, (byte)text.Color.Green, (byte)text.Color.Blue, (byte)text.Color.Alpha);

            using var paint = new SKPaint() { Color = color, IsAntialias = true };
            paint.IsAntialias = antiAlias;

            var typeFace = SKTypeface.FromFamilyName(text.FontFamily.Name);
            var font = new SKFont(typeFace, (float)text.FontSize);

            var textBlob = SKTextBlob.Create(text.Text, font);
            if (textBlob is null)
            {
                return;
            }

            canvas.DrawText(textBlob, (float)text.TopLeftX, (float)text.BottomLeftY, paint);
        }

        protected override void DrawElement(SKCanvas canvas, DrawableEllipse ellipse)
        {
            var color = new SKColor((byte)ellipse.Color.Red, (byte)ellipse.Color.Green, (byte)ellipse.Color.Blue, (byte)ellipse.Color.Alpha);
            var paint = new SKPaint() { Color = color, IsAntialias = true };
            paint.IsAntialias = antiAlias;

            canvas.DrawOval((float)ellipse.CenterX, (float)ellipse.CenterY, (float)ellipse.Width / 2, (float)ellipse.Height / 2, paint);
            if (ellipse.StrokeColor != null && ellipse.StrokeWeight > 0)
            {
                color = new SKColor((byte)ellipse.StrokeColor.Red, (byte)ellipse.StrokeColor.Green, (byte)ellipse.StrokeColor.Blue, (byte)ellipse.StrokeColor.Alpha);
                paint = new SKPaint() { Color = color, IsStroke = true, StrokeWidth = (float)ellipse.StrokeWeight };

                canvas.DrawOval((float)ellipse.CenterX, (float)ellipse.CenterY, (float)ellipse.Width / 2, (float)ellipse.Height / 2, paint);
            }
        }

        protected override void DrawElement(SKCanvas canvas, DrawablePolyline polyline)
        {
            if (!polyline.Points.Any())
            {
                return;
            }

            var color = new SKColor((byte)polyline.Color.Red, (byte)polyline.Color.Green, (byte)polyline.Color.Blue, (byte)polyline.Color.Alpha);

            using var paint = new SKPaint() { Color = color, IsStroke = true, Style = SKPaintStyle.Stroke, StrokeWidth = (float)polyline.StrokeWeight, StrokeJoin = SKStrokeJoin.Round, StrokeCap = SKStrokeCap.Butt, IsAntialias = true };
            paint.IsAntialias = antiAlias;

            path.Reset();
            path.MoveTo(polyline.Points.First().ToSkiaPoint());

            foreach (var point in polyline.Points.Skip(1).Select(p => p.ToSkiaPoint()))
            {
                path.LineTo(point);
            }

            canvas.DrawPath(path, paint);
        }

        protected override void DrawElement(SKCanvas canvas, DrawablePolygon polygon)
        {
            path.Reset();

            path.MoveTo(polygon.Points.First().ToSkiaPoint());

            foreach (var point in polygon.Points.Skip(1).Select(P => P.ToSkiaPoint()))
            {
                path.LineTo(point);
            }

            path.Close();

            if (polygon.Fill != null)
            {
                var fillColor = new SKColor((byte)polygon.Fill.Red, (byte)polygon.Fill.Green, (byte)polygon.Fill.Blue, (byte)polygon.Fill.Alpha);

                using var fillPaint = new SKPaint() { Color = fillColor, IsStroke = false, Style = SKPaintStyle.Fill, IsAntialias = true };
                fillPaint.IsAntialias = antiAlias;

                canvas.DrawPath(path, fillPaint);
            }

            if (polygon.Color != null && polygon.StrokeWeight > 0)
            {
                var strokeColor = new SKColor((byte)polygon.Color.Red, (byte)polygon.Color.Green, (byte)polygon.Color.Blue, (byte)polygon.Color.Alpha);

                using var strokePaint = new SKPaint() { Color = strokeColor, IsStroke = true, Style = SKPaintStyle.Stroke, StrokeJoin = SKStrokeJoin.Miter, StrokeCap = SKStrokeCap.Butt, IsAntialias = true };
                strokePaint.IsAntialias = antiAlias;

                canvas.DrawPath(path, strokePaint);
            }
        }

        protected override void DrawElement(SKCanvas canvas, DrawableBezierCurve bezier)
        {
            throw new NotImplementedException();
        }
    }
}
