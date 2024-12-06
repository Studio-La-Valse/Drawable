using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// The base class for all drawable elements. 
    /// Provides base functionality required by the <see cref="SceneManager{TEntity, TKey}"/>. 
    /// Specified coordinates are relative to the top left of the canvas. Positive x- and y values visually represent right- and downwards translation respectively.
    /// </summary>
    public abstract class BaseDrawableElement
    {
        /// <summary>
        /// Calculates the bounding box. Optimizations may be provided per geometric primitive.
        /// </summary>
        /// <returns></returns>
        public abstract BoundingBox GetBoundingBox();

        /// <summary>
        /// Returns a new position that is the closest point on the edges of the element relative to the specified point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract XY ClosestPointEdge(XY other);

        /// <summary>
        /// Returns a new position that is the closest point inside of the shape of the element relative to the specified point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract XY ClosestPointShape(XY other);

        /// <summary>
        /// Returns a boolean value whether the drawable element contains the specified position with an optional margin.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public bool ContainsPosition(XY position, double? margin = null)
        {
            var distance = ClosestPointShape(position).DistanceTo(position);
            margin ??= BaseGeometry.threshold;
            distance -= margin.Value;
            var contains = distance <= 0;
            return contains;
        }
    }
}
