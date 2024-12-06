using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.HTML.Extensions;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.HTML
{
    /// <summary>
    /// An experimental implementation of the <see cref="BaseBitmapPainter{TBitmap}"/> that draws drawable elements into a html/svg string.
    /// </summary>
    public class HTMLCanvasPainter : BaseLazyBitmapPainter<HTMLCanvas>
    {
        private readonly HTMLCanvas canvas;

        /// <inheritdoc/>
        public HTMLCanvasPainter(HTMLCanvas canvas) : base(canvas)
        {
            this.canvas = canvas;
        }


        /// <inheritdoc/>
        public override void DrawBackground(ColorARGB color)
        {
            canvas.Background = color;
        }

        /// <inheritdoc/>
        public override void InitDrawing()
        {
            canvas.Clear();
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawableLine line)
        {
            var svg = line.Svg();
            canvas.Add(svg);
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawableRectangle rectangle)
        {
            var svg = rectangle.Svg();
            canvas.Add(svg);
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawableText text)
        {
            var svg = text.Svg();
            canvas.Add(svg);
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawableEllipse ellipse)
        {
            var svg = ellipse.Svg();
            canvas.Add(svg);
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawablePolyline polyline)
        {
            var svg = polyline.Svg();
            canvas.Add(svg);
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawablePolygon polygon)
        {
            var svg = polygon.Svg();
            canvas.Add(svg);
        }

        /// <inheritdoc/>
        protected override void DrawElement(HTMLCanvas canvas, DrawableBezierQuadratic bezier)
        {
            throw new NotImplementedException();
        }
    }
}
