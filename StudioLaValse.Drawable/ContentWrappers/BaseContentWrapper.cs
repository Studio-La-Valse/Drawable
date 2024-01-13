using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.ContentWrappers
{
    /// <summary>
    /// The base class for all content wrappers. Used to build a visual representation of a model.
    /// </summary>
    public abstract class BaseContentWrapper
    {
        /// <summary>
        /// Virtual method that provides a bounding box for the virtual element. By default, creates a bounding box for all drawable elements returned by <see cref="GetDrawableElements"/>
        /// </summary>
        /// <returns></returns>
        public virtual BoundingBox BoundingBox()
        {
            return new BoundingBox(GetDrawableElements().Select(element => element.GetBoundingBox()));
        }

        /// <summary>
        /// Override this method to provide the <see cref="BaseDrawableElement"/>s that represents the visual model in this layer.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<BaseDrawableElement> GetDrawableElements();

        /// <summary>
        /// Called to get all child <see cref="BaseContentWrapper"/>s of this layer. Implement to create a tree-like visual representaion of the model.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<BaseContentWrapper> GetContentWrappers();
    }
}
