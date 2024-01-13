namespace StudioLaValse.Drawable.Private
{
    internal class EntityInvalidator<TEntity> : INotifyEntityChanged<TEntity>
    {
        private readonly HashSet<IObserver<TEntity>> _observers = new();

        public EntityInvalidator() { }

        public void Invalidate(TEntity element)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(element);
            }
        }

        public void Invalidate(IEnumerable<TEntity> elements)
        {
            foreach (var element in elements)
            {
                Invalidate(element);
            }
        }

        public void Invalidate(params TEntity[] elements)
        {
            foreach (var element in elements)
            {
                Invalidate(element);
            }
        }

        public void RenderChanges()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<TEntity> observer)
        {
            _observers.Add(observer);
            return new DefaultUnsubscriber<TEntity>(observer, _observers);
        }
    }
}
