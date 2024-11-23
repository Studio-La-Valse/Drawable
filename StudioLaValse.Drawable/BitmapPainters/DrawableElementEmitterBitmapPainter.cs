using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.BitmapPainters
{
    /// <summary>
    /// An implementation of the <see cref="BaseBitmapPainter"/> that does not draw to canvas, but instead emits the values via a <see cref="DrawableElementEmitter"/>.
    /// </summary>
    public class DrawableElementEmitterBitmapPainter : BaseBitmapPainter
    {
        private readonly DrawableElementEmitter drawableElementEmitter;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="drawableElementEmitter"></param>
        public DrawableElementEmitterBitmapPainter(DrawableElementEmitter drawableElementEmitter)
        {
            this.drawableElementEmitter = drawableElementEmitter;
        }

        /// <summary>
        /// Does not actually do anything.
        /// </summary>
        /// <param name="color"></param>
        public override void DrawBackground(ColorARGB color)
        {

        }

        /// <inheritdoc/>
        public override void DrawElement(BaseDrawableElement element)
        {
            drawableElementEmitter.Emit(element);
        }

        /// <inheritdoc/>
        public override void FinishDrawing()
        {
            drawableElementEmitter.Complete();
        }

        /// <summary>
        /// Does not actually do anything.
        /// </summary>
        public override void InitDrawing()
        {

        }
    }
}
