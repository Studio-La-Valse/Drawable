using StudioLaValse.Drawable.DrawableElements;

namespace StudioLaValse.Drawable.BitmapPainters
{
    /// <summary>
    /// An abstract implementation of the <see cref="BaseBitmapPainter{TBitmap}"/> that adds draw requests to a list so that the same batch request can be executed multiple times. The cache is cleared when <see cref="InitDrawing"/> is called.
    /// </summary>
    /// <typeparam name="TBitmap">Type of bitmap</typeparam>
    public abstract class BaseCachingBitmapPainter<TBitmap> : BaseBitmapPainter<TBitmap>
    {
        /// <summary>
        /// The cache where the draw requests are added to.
        /// </summary>
        protected abstract List<Action<TBitmap>> Cache { get; }



        /// <summary>
        /// Clears the cache.
        /// </summary>
        public override void InitDrawing()
        {
            Cache.Clear();
        }

        /// <summary>
        /// Adds a draw action for the specified <see cref="BaseDrawableElement"/> to the cache, does not execute until the associated bitmap is ready to consume the cache.
        /// </summary>
        /// <param name="element"></param>
        /// <exception cref="NotSupportedException"></exception>
        public sealed override void DrawElement(BaseDrawableElement element)
        {
            switch (element)
            {
                case DrawableLine line:
                    Cache.Add((c) => DrawElement(c, line));
                    break;

                case DrawableRectangle rectangle:
                    Cache.Add((c) => DrawElement(c, rectangle));
                    break;

                case DrawableText text:
                    Cache.Add((c) => DrawElement(c, text));
                    break;

                case DrawableEllipse ellipse:
                    Cache.Add((c) => DrawElement(c, ellipse));
                    break;

                case DrawableBezierQuadratic bezier:
                    Cache.Add(c => DrawElement(c, bezier));
                    break;

                case DrawableBezierCubic bezier:
                    Cache.Add(c => DrawElement(c, bezier));
                    break;

                case DrawablePolygon polygon:
                    Cache.Add((c) => DrawElement(c, polygon));
                    break;

                case DrawablePolyline polyline:
                    Cache.Add((c) => DrawElement(c, polyline));
                    break;

                default:
                    throw new NotSupportedException($"Element {nameof(element)}, type {element.GetType()} is not a supported drawable element.");
            }
        }
    }
}
