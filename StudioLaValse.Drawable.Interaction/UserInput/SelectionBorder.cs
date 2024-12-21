using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction
{
    /// <summary>
    /// A selection border that may be observed.
    /// </summary>
    public class SelectionBorder : IObservable<BoundingBox>
    {
        private readonly HashSet<IObserver<BoundingBox>> observers = [];

        /// <inheritdoc/>
        public IDisposable Subscribe(IObserver<BoundingBox> observer)
        {
            observers.Add(observer);
            return new DefaultUnsubscriber<BoundingBox>(observer, observers);
        }
        
        /// <summary>
        /// Set the specified bounding box to be the next selection border.
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
        /// Hide the selection border. 
        /// </summary>
        public void Hide()
        {
            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
        }
    }
}

