using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.UserInput
{
    /// <summary>
    /// An <see cref="IObservable{T}"/> of type <see cref="BoundingBox"/> that can be set or hidden.
    /// </summary>
    public class ObservableBoundingBox : IObservable<BoundingBox>
    {
        private readonly ISet<IObserver<BoundingBox>> observers = new HashSet<IObserver<BoundingBox>>();

        public ObservableBoundingBox()
        {

        }

        /// <summary>
        /// Emits a new <see cref="BoundingBox"/> value to the subscribers.
        /// </summary>
        /// <param name="boundingBox"></param>
        public void Set(BoundingBox boundingBox)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(boundingBox);
            }
        }

        /// <summary>
        /// Calls for all subscribers to <see cref="IObserver{T}.OnCompleted"/>.
        /// </summary>
        public void Hide()
        {
            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
        }

        /// <summary>
        /// Subscribes the specified <see cref="IObserver{T}"/> to this instance.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<BoundingBox> observer)
        {
            observers.Add(observer);
            return new DefaultUnsubscriber<BoundingBox>(observer, observers);
        }
    }
}
