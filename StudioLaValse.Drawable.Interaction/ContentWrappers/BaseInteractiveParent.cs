using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{

    /// <summary>
    /// An abstract class meant to be used for a visual parent that needs any basic mouse interaction. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseInteractiveParent<TEntity> : BaseVisualParent<TEntity>, IBehaviorElement<TEntity> where TEntity : class
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
        protected bool IsMouseOver { get; set; }

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
        public virtual InvalidationRequest<TEntity>? OnMouseEnter()
        {
            if (IsMouseOver)
            {
                return null;
            }

            IsMouseOver = true;
            return new InvalidationRequest<TEntity>(Ghost, Method: Method.Recursive);
        }

        /// <summary>
        /// Custom logic on mouse enter.
        /// </summary>
        /// <returns></returns>
        public virtual InvalidationRequest<TEntity>? OnMouseLeave()
        {
            if (!IsMouseOver)
            {
                return null;
            }

            IsMouseOver = false;
            return new InvalidationRequest<TEntity>(Ghost, Method: Method.Recursive);
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
        public virtual bool HandleLeftMouseButtonDown(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonUp(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleRightMouseButtonDown(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleRightMouseButtonUp(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleSetMousePosition(XY position, Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            LastMousePosition = position;

            var wasOverBefore = IsMouseOver;
            var isNowOver = CaptureMouse(LastMousePosition);

            var respond = wasOverBefore != isNowOver;
            if (respond)
            {
                if (isNowOver)
                {
                    if (OnMouseEnter() is InvalidationRequest<TEntity> e)
                    {
                        invalidationRequests.Enqueue(e);
                    }
                }
                else
                {
                    if (OnMouseLeave() is InvalidationRequest<TEntity> e)
                    {
                        invalidationRequests.Enqueue(e);
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleMouseWheel(double delta, Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleKeyUp(Key key, Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleKeyDown(Key key, Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            return true;
        }
    }
}
