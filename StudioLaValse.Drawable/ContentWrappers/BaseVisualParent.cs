namespace StudioLaValse.Drawable.ContentWrappers
{
    /// <summary>
    /// An abstract implementation of the <see cref="BaseContentWrapper"/> that has an attached entity. Invalidation of the specified entity by the <see cref="SceneManager{TKey}"/> will start from this layer in the visual tree.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseVisualParent<TKey> : BaseContentWrapper where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The element associated with this <see cref="BaseVisualParent{TEntity}"/>.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="element"></param>
        public BaseVisualParent(TKey element)
        {
            Key = element;
        }
    }
}
