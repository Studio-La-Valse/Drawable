namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An interface required to notify and <see cref="IObserver{T}"></see> that the instance has chaned. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INotifyEntityChanged<TEntity> : IObservable<TEntity>
    {
        /// <summary>
        /// Notifies listeners that an entity has changed.
        /// </summary>
        /// <param name="element"></param>
        void Invalidate(TEntity element);
        /// <summary>
        /// Notifies listeners that a number of entities has changed.
        /// </summary>
        /// <param name="elements"></param>
        void Invalidate(IEnumerable<TEntity> elements);
        /// <summary>
        /// Notifies listeners that a number of entities has changed.
        /// </summary>
        /// <param name="elements"></param>
        void Invalidate(params TEntity[] elements);
        /// <summary>
        /// Notifies listeners that no more entities will change for now, and that a render update may be called.
        /// </summary>
        void RenderChanges();
    }
}
