using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Private;

namespace StudioLaValse.Drawable.BitmapPainters
{
    /// <summary>
    /// A generic purpose <see cref="IObservable{BaseDrawableElement}"/> that emits <see cref="BaseDrawableElement"/> values.
    /// </summary>
    public class DrawableElementEmitter : IObservable<BaseDrawableElement>
    {
        private readonly HashSet<IObserver<BaseDrawableElement>> observers = [];

        /// <summary>
        /// The default constructor.
        /// </summary>
        public DrawableElementEmitter()
        {

        }

        /// <summary>
        /// Emits a new value.
        /// </summary>
        /// <param name="baseDrawableElement"></param>
        public void Emit(BaseDrawableElement baseDrawableElement)
        {
            foreach (var subscriber in observers)
            {
                subscriber.OnNext(baseDrawableElement);
            }
        }

        /// <summary>
        /// Signals to the subscribers that the emissions are completed.
        /// </summary>
        public void Complete()
        {
            foreach (var subscriber in observers)
            {
                subscriber.OnCompleted();
            }
        }

        /// <inheritdoc/>
        public IDisposable Subscribe(IObserver<BaseDrawableElement> observer)
        {
            return Unsubscriber<BaseDrawableElement>.SubscribeOrThrow(observers, observer);
        }
    }
}
