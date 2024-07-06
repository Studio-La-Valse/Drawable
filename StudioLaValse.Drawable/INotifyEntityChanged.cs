namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An interface required to notify and <see cref="IObserver{T}"></see> that the instance has chaned. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INotifyEntityChanged<TEntity> : IObservable<InvalidationRequest<TEntity>>
    {
        /// <summary>
        /// Notifies listeners that an entity has changed.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="notFoundHandler"></param>
        /// <param name="method"></param>
        void Invalidate(TEntity element, NotFoundHandler notFoundHandler = NotFoundHandler.Raise, Method method = Method.Recursive);
        /// <summary>
        /// Notifies listeners that a number of entities has changed.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="notFoundHandler"></param>
        /// <param name="method"></param>
        void Invalidate(IEnumerable<TEntity> elements, NotFoundHandler notFoundHandler = NotFoundHandler.Raise, Method method = Method.Recursive);
        /// <summary>
        /// Notifies listeners that a number of entities has changed.
        /// </summary>
        /// <param name="notFoundHandler"></param>
        /// <param name="method"></param>
        /// <param name="elements"></param>
        void Invalidate(NotFoundHandler notFoundHandler = NotFoundHandler.Raise, Method method = Method.Recursive, params TEntity[] elements);
        /// <summary>
        /// Notifies listeners that no more entities will change for now, and that a render update may be called.
        /// </summary>
        void RenderChanges();
    }
}
