namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An interface required to notify and <see cref="IObserver{T}"></see> that the instance has chaned. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INotifyEntityChanged<TEntity> : IObservable<TEntity>
    {
        void Invalidate(TEntity element);
        void Invalidate(IEnumerable<TEntity> elements);
        void Invalidate(params TEntity[] elements);
        void RenderChanges();
    }
}
