namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An interface required to notify and <see cref="IObserver{T}"></see> that the instance has chaned. 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface INotifyEntityChanged<TKey> : IObservable<InvalidationRequest<TKey>> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Notifies listeners that an entity has changed.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="notFoundHandler"></param>
        /// <param name="renderMethod"></param>
        void Invalidate(TKey element, NotFoundHandler notFoundHandler = NotFoundHandler.Throw, RenderMethod renderMethod = RenderMethod.Recursive);
        /// <summary>
        /// Notifies listeners that a number of entities has changed.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="notFoundHandler"></param>
        /// <param name="renderMethod"></param>
        void Invalidate(IEnumerable<TKey> elements, NotFoundHandler notFoundHandler = NotFoundHandler.Throw, RenderMethod renderMethod = RenderMethod.Recursive);
        /// <summary>
        /// Notifies listeners that a number of entities has changed.
        /// </summary>
        /// <param name="notFoundHandler"></param>
        /// <param name="renderMethod"></param>
        /// <param name="elements"></param>
        void Invalidate(NotFoundHandler notFoundHandler = NotFoundHandler.Throw, RenderMethod renderMethod = RenderMethod.Recursive, params TKey[] elements);
        /// <summary>
        /// Notifies listeners that no more entities will change for now, and that a render update may be called.
        /// </summary>
        void RenderChanges();
    }
}
