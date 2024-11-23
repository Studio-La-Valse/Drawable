namespace StudioLaValse.Drawable.Private
{
    internal class Unsubscriber<T> : IDisposable
    {
        private readonly ICollection<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        private Unsubscriber(ICollection<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            _observers.Remove(_observer);
        }

        public static IDisposable SubscribeOrThrow(HashSet<IObserver<T>> observers, IObserver<T> observer)
        {
            if (!observers.Add(observer))
            {
                throw new InvalidOperationException();
            }

            return new Unsubscriber<T>(observers, observer);
        }
    }
}
