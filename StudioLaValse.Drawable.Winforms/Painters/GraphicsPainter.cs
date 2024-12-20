using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.Winforms.Controls;
using StudioLaValse.Drawable.Winforms.Extensions;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Winforms.Painters
{
    public class GraphicsPainter : BaseCachingBitmapPainter<Graphics>
    {
        private readonly ControlContainer drawingContext;
        private readonly IMeasureText textMeasurer;

        protected override List<Action<Graphics>> Cache => drawingContext.DrawActions;

        public GraphicsPainter(ControlContainer drawingContext, IMeasureText textMeasurer)
        {
            this.drawingContext = drawingContext;
            this.textMeasurer = textMeasurer;
        }


        public override void DrawBackground(ColorARGB colorARGB)
        {
            drawingContext.BackColor = colorARGB.ToWindowsColor();
        }

        protected override void DrawElement(Graphics drawingContext, DrawableLine line)
        {
            var pen = new Pen(line.Color.ToWindowsColor(), (float)line.Thickness);
            drawingContext.DrawLine(pen, line.TopLeft.ToWindowsPoint(), line.BottomRight.ToWindowsPoint());
        }

        protected override void DrawElement(Graphics drawingContext, DrawableRectangle rectangle)
        {
            var rect = new Rectangle(new Point((int)rectangle.TopLeftX, (int)rectangle.TopLeftY), new Size((int)rectangle.Width, (int)rectangle.Height));
            drawingContext.FillRectangle(rectangle.Color.ToWindowsBrush(), rect);

            if (rectangle.StrokeColor != null && rectangle.StrokeWeight > 0)
            {
                var pen = new Pen(rectangle.StrokeColor.Value.ToWindowsColor(), (float)rectangle.StrokeWeight);
                drawingContext.DrawRectangle(pen, rect);
            }
        }

        protected override void DrawElement(Graphics drawingContext, DrawableText text)
        {
            drawingContext.DrawString(
                text.Text,
                text.FontFamily.ToWindowsFont(text.FontSize),
                text.Color.ToWindowsBrush(),
                (float)text.GetLeft(textMeasurer),
                (float)text.GetTop(textMeasurer));
        }

        protected override void DrawElement(Graphics drawingContext, DrawableEllipse ellipse)
        {
            var brush = ellipse.Color.ToWindowsBrush();
            var x = (float)(ellipse.CenterX - ellipse.Width / 2);
            var y = (float)(ellipse.CenterY - ellipse.Height / 2);
            var width = (float)ellipse.Width;
            var height = (float)ellipse.Height;
            drawingContext.FillEllipse(brush, x, y, width, height);

            if (ellipse.StrokeColor != null && ellipse.StrokeWeight > 0)
            {
                var strokeBrush = ellipse.StrokeColor.Value.ToWindowsBrush();
                var pen = new Pen(strokeBrush, (float)ellipse.StrokeWeight);
                drawingContext.DrawEllipse(pen, (float)(ellipse.CenterX - ellipse.Width / 2), (float)(ellipse.CenterY - ellipse.Height / 2), (float)ellipse.Width, (float)ellipse.Height);
            }
        }

        protected override void DrawElement(Graphics drawingContext, DrawablePolyline polyline)
        {
            throw new NotImplementedException();
        }

        protected override void DrawElement(Graphics drawingContext, DrawablePolygon polygon)
        {
            throw new NotImplementedException();
        }

        public override void FinishDrawing()
        {
            drawingContext.Invalidate();
        }

        protected override void DrawElement(Graphics canvas, DrawableBezierQuadratic bezier)
        {
            throw new NotImplementedException();
        }

        protected override void DrawElement(Graphics canvas, DrawableBezierCubic bezier)
        {
            throw new NotImplementedException();
        }
    }
}
