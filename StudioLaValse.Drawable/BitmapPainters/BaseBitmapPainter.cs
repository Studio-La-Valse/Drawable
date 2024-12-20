using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.BitmapPainters
{
    /// <summary>
    /// The base class for all bitmap painters. Required by the <see cref="SceneManager{TKey}"/>
    /// </summary>
    public abstract class BaseBitmapPainter
    {
        /// <summary>
        /// Called before dispatching any draw request. Override to implement any behaviour before drawing to target.
        /// </summary>
        public abstract void InitDrawing();

        /// <summary>
        /// Called before any batch draw request.
        /// </summary>
        /// <param name="color"></param>
        public abstract void DrawBackground(ColorARGB color);

        /// <summary>
        /// Abstract method to draw a drawable element to a target.
        /// </summary>
        /// <param name="element"></param>
        public abstract void DrawElement(BaseDrawableElement element);

        /// <summary>
        /// Called when all draw requests in the batch are completed. Override to implement any behaviour after completing the last draw request.
        /// </summary>
        public abstract void FinishDrawing();
    }

    /// <summary>
    /// The base class for all bitmap painters that have a specific type of bitmap target.
    /// </summary>
    /// <typeparam name="TBitmap"></typeparam>
    public abstract class BaseBitmapPainter<TBitmap> : BaseBitmapPainter
    {
        /// <summary>
        /// Draw a line to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="line"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawableLine line);

        /// <summary>
        /// Draw a rectangle to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rectangle"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawableRectangle rectangle);

        /// <summary>
        /// Draw text to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="text"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawableText text);

        /// <summary>
        /// Draw an ellipse to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="ellipse"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawableEllipse ellipse);

        /// <summary>
        /// Draw a polyline to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="polyline"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawablePolyline polyline);

        /// <summary>
        /// Draw a polygon to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="polygon"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawablePolygon polygon);

        /// <summary>
        /// Draw a bezier curve to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="bezier"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawableBezierQuadratic bezier);

        /// <summary>
        /// Draw a bezier curve to the provided bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="bezier"></param>
        protected abstract void DrawElement(TBitmap bitmap, DrawableBezierCubic bezier);
    }
}
