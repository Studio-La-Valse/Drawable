using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// An abstract class for visual elements that can be transformed.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseTransformableParent<TEntity> : BaseSelectableParent<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        protected BaseTransformableParent(TEntity element, ISelection<TEntity> selection) : base(element, selection)
        {

        }

        /// <summary>
        /// Called for every update of a transformation.
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <returns>A boolean value to indicate wether the update should require a rerender of the element returned by <see cref="OnTransformInvalidate"/> method.</returns>
        public abstract bool Transform(double deltaX, double deltaY);

        /// <summary>
        /// A method called during transformation to check which item to rerender.
        /// By default returns a reference to the <see cref="BaseVisualParent{TEntity}.AssociatedElement"/>
        /// </summary>
        /// <returns></returns>
        public virtual TEntity OnTransformInvalidate()
        {
            return AssociatedElement;
        }
    }
}
