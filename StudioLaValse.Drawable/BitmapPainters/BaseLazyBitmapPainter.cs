using StudioLaValse.Drawable.DrawableElements;

namespace StudioLaValse.Drawable.BitmapPainters
{
    /// <summary>
    /// An abstract implementation of the <see cref="BaseBitmapPainter{TBitmap}"/> that does not draw to the bitmap directly, but queues each request to be executed later.
    /// </summary>
    /// <typeparam name="TBitmap"></typeparam>
    public abstract class BaseLazyBitmapPainter<TBitmap> : BaseBitmapPainter<TBitmap>
    {
        private readonly Queue<Action<TBitmap>> drawActions = new();
        private readonly TBitmap canvas;


        protected BaseLazyBitmapPainter(TBitmap canvas)
        {
            this.canvas = canvas;
        }


        /// <summary>
        /// Adds a draw action for the specified <see cref="BaseDrawableElement"/> to the cache. Does not execute the dispatched draw request until the specified bitmap is ready to consume the queue.
        /// </summary>
        /// <param name="element"></param>
        /// <exception cref="NotSupportedException"></exception>
        public sealed override void DrawElement(BaseDrawableElement element)
        {
            switch (element)
            {
                case DrawableLine line:
                    drawActions.Enqueue((c) => DrawElement(c, line));
                    break;

                case DrawableRectangle rectangle:
                    drawActions.Enqueue((c) => DrawElement(c, rectangle));
                    break;

                case DrawableText text:
                    drawActions.Enqueue((c) => DrawElement(c, text));
                    break;

                case DrawableEllipse ellipse:
                    drawActions.Enqueue((c) => DrawElement(c, ellipse));
                    break;

                case DrawableBezierCurve curve:
                    drawActions.Enqueue(c => DrawElement(c, curve));
                    break;

                case DrawablePolygon polygon:
                    drawActions.Enqueue((c) => DrawElement(c, polygon));
                    break;

                case DrawablePolyline polyline:
                    drawActions.Enqueue((c) => DrawElement(c, polyline));
                    break;

                default:
                    throw new NotSupportedException($"Element {nameof(element)}, type {element.GetType()} is not a supported drawable element.");
            }
        }


        /// <summary>
        /// Dequeues all pending draw actions and dispatches them to the associated bitmap.
        /// </summary>
        public sealed override void FinishDrawing()
        {
            while (drawActions.Count != 0)
            {
                var action = drawActions.Dequeue();
                action.Invoke(canvas);
            }
        }
    }
}
