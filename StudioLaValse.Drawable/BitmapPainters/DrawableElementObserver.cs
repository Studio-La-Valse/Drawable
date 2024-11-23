using StudioLaValse.Drawable.DrawableElements;

namespace StudioLaValse.Drawable.BitmapPainters
{
    /// <summary>
    /// A generic purpose <see cref="IObserver{BaseDrawableElement}"/> that dispatches drawable elements to a injected <see cref="BaseBitmapPainter"/>.
    /// </summary>
    public class DrawableElementObserver : IObserver<BaseDrawableElement>
    {
        private readonly BaseBitmapPainter baseBitmapPainter;

        private bool completed = true;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="baseBitmapPainter"></param>
        public DrawableElementObserver(BaseBitmapPainter baseBitmapPainter)
        {
            this.baseBitmapPainter = baseBitmapPainter;
        }

        /// <inheritdoc/>
        public void OnCompleted()
        {
            baseBitmapPainter.FinishDrawing();

            completed = true;
        }

        /// <inheritdoc/>
        public void OnError(Exception error)
        {
            throw error;
        }

        /// <inheritdoc/>
        public void OnNext(BaseDrawableElement value)
        {
            if (completed)
            {
                baseBitmapPainter.InitDrawing();
                completed = false;
            }

            baseBitmapPainter.DrawElement(value);
        }
    }
}
