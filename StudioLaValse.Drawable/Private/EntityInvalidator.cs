namespace StudioLaValse.Drawable.Private
{
    internal class EntityInvalidator<TKey> : INotifyEntityChanged<TKey> where TKey : IEquatable<TKey>
    {
        private readonly HashSet<IObserver<InvalidationRequest<TKey>>> _observers = [];

        public EntityInvalidator() { }

        public void Invalidate(TKey invalidationRequest, NotFoundHandler notFoundHandler, RenderMethod method)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new InvalidationRequest<TKey>(invalidationRequest, notFoundHandler, method));
            }
        }

        public void Invalidate(IEnumerable<TKey> invalidationRequests, NotFoundHandler notFoundHandler, RenderMethod method)
        {
            foreach (var element in invalidationRequests)
            {
                Invalidate(element, notFoundHandler, method);
            }
        }

        public void Invalidate(NotFoundHandler notFoundHandler, RenderMethod method, params TKey[] invalidationRequests)
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

        public IDisposable Subscribe(IObserver<InvalidationRequest<TKey>> observer)
        {
            _observers.Add(observer);
            return new DefaultUnsubscriber<InvalidationRequest<TKey>>(observer, _observers);
        }
    }
}
