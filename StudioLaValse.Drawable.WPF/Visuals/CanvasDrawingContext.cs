using System.Windows;
using System.Windows.Media;

namespace StudioLaValse.Drawable.WPF.Visuals
{
    /// <summary>
    /// An implementation of the <see cref="FrameworkElement"/> to provide capability to draw to a <see cref="VisualCollection"/>.
    /// </summary>
    public class CanvasDrawingContext : FrameworkElement
    {
        private readonly VisualCollection children;
        private readonly DrawingVisual drawingVisual;


        protected override int VisualChildrenCount => children.Count;


        public CanvasDrawingContext()
        {
            drawingVisual = new DrawingVisual();
            children = new VisualCollection(this)
            {
                drawingVisual
            };
        }




        protected override Size MeasureOverride(Size availableSize)
        {
            if (drawingVisual.ContentBounds.Size == Size.Empty)
            {
                return new Size(0, 0);
            }

            return new Size(
                drawingVisual.ContentBounds.Right,
                drawingVisual.ContentBounds.Bottom);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return children[index];
        }

        /// <summary>
        /// Calling this method opens the <see cref="DrawingVisual"/> object for rendering. Use this object to render into the <see cref="DrawingVisual"/>.
        /// </summary>
        /// <returns></returns>
        public DrawingContext RenderOpen() => drawingVisual.RenderOpen();
    }
}
