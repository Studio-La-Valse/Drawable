namespace StudioLaValse.Drawable.Private
{
    internal class EntityInvalidator<TEntity> : INotifyEntityChanged<TEntity>
    {
        private readonly HashSet<IObserver<InvalidationRequest<TEntity>>> _observers = [];

        public EntityInvalidator() { }

        public void Invalidate(TEntity invalidationRequest, NotFoundHandler notFoundHandler, Method method)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new InvalidationRequest<TEntity>(invalidationRequest, notFoundHandler, method));
            }
        }

        public void Invalidate(IEnumerable<TEntity> invalidationRequests, NotFoundHandler notFoundHandler, Method method)
        {
            foreach (var element in invalidationRequests)
            {
                Invalidate(element, notFoundHandler, method);
            }
        }

        public void Invalidate(NotFoundHandler notFoundHandler, Method method, params TEntity[] invalidationRequests)
        {
            foreach (var element in invalidationRequests)
            {
                Invalidate(element, notFoundHandler, method);
            }
        }

        public void RenderChanges()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<InvalidationRequest<TEntity>> observer)
        {
            _observers.Add(observer);
            return new DefaultUnsubscriber<InvalidationRequest<TEntity>>(observer, _observers);
        }
    }
}
