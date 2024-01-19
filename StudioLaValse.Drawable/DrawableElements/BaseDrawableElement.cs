using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// The base class for all drawable elements. 
    /// Provides base functionality required by the <see cref="SceneManager{TEntity}"/>. 
    /// Specified coordinates are relative to the top left of the canvas. Positive x- and y values visually represent right- and downwards translation respectively.
    /// </summary>
    public abstract class BaseDrawableElement
    {
        /// <summary>
        /// Calculates the bounding box. Optimizations may be provided per geometric primitive.
        /// </summary>
        /// <returns></returns>
        public abstract BoundingBox GetBoundingBox();
    }
}
