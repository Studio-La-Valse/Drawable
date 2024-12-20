using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// Defines a visual parent that has the potential to assign behavior to.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseInteractiveParent<TKey> : BaseVisualParent<TKey>, IInputObserver where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The last recorded mouse position.
        /// </summary>
        protected XY LastMousePosition { get; set; }

        /// <summary>
        /// The last position when the left mouse button was pressed.
        /// </summary>
        protected XY LastPositionOnMouseLeftDown { get; set; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="element"></param>
        protected BaseInteractiveParent(TKey element) : base(element)
        {
            
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonDown()
        {
            LastPositionOnMouseLeftDown = LastMousePosition;
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonUp()
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleRightMouseButtonDown()
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleRightMouseButtonUp()
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleMouseMove(XY position)
        {
            LastMousePosition = position;
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleMouseWheel(double delta)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleKeyUp(Key key)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleKeyDown(Key key)
        {
            return true;
        }
    }
}
