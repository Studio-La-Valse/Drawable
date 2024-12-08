using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Reflection.Metadata.Ecma335;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{

    /// <summary>
    /// An abstract class meant to be used for a visual parent that needs any basic mouse interaction. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseInteractiveParent<TEntity> : BaseVisualParent<TEntity>, IInputObserver where TEntity : class
    {
        /// <summary>
        /// A reference to an entity that will be rerendered on mouse events. 
        /// The default value is a reference to the original associated entity, but for complex nested entities,
        /// you can reference another entity to greately reduce calculation times.
        /// </summary>
        public virtual TEntity Ghost => AssociatedElement;

        /// <summary>
        /// A boolean value indicating wether or not the cursor is currently above the visual element.
        /// </summary>
        protected virtual bool IsMouseOver { get; set; }

        /// <summary>
        /// The last recorded mouse position.
        /// </summary>
        protected XY LastMousePosition { get; set; }


        /// <inheritdoc/>
        protected BaseInteractiveParent(TEntity element) : base(element)
        {

        }

        /// <summary>
        /// Custom logic on mouse enter.
        /// </summary>
        /// <returns></returns>
        public virtual void OnMouseEnter()
        {
            if (IsMouseOver)
            {
                return;
            }

            IsMouseOver = true;
        }

        /// <summary>
        /// Custom logic on mouse enter.
        /// </summary>
        /// <returns></returns>
        public virtual void OnMouseLeave()
        {
            if (!IsMouseOver)
            {
                return;
            }

            IsMouseOver = false;
        }

        /// <summary>
        /// An abstract method called to check wether the current mouse position should trigger any kind of response.
        /// By default, this method returns true if the position is contained in the default bounding box of the content wrapper.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool CaptureMouse(XY point)
        {
            return GetDrawableElements().Any(e => e.ContainsPosition(point));
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonDown()
        {
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
        public virtual bool HandleSetMousePosition(XY position)
        {
            LastMousePosition = position;

            var wasOverBefore = IsMouseOver;
            var isNowOver = CaptureMouse(position);

            if(wasOverBefore != isNowOver)
            {
                if (isNowOver)
                {
                    OnMouseEnter();
                }
                else
                {
                    OnMouseLeave();
                }
            }
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
