namespace StudioLaValse.Drawable.ContentWrappers
{
    /// <summary>
    /// An abstract implementation of the <see cref="BaseContentWrapper"/> that has an attached entity. Invalidation of the specified entity by the <see cref="SceneManager{TEntity, TKey}"/> will start from this layer in the visual tree.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseVisualParent<TEntity> : BaseContentWrapper where TEntity : class
    {
        /// <summary>
        /// The element associated with this <see cref="BaseVisualParent{TEntity}"/>.
        /// </summary>
        public TEntity AssociatedElement { get; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="element"></param>
        public BaseVisualParent(TEntity element)
        {
            AssociatedElement = element;
        }
    }
}
