using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// An abstract class for visual elements that can be transformed.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseTransformableParent<TEntity> : BaseSelectableParent<TEntity> where TEntity : class
    {
        private bool leftMouseIsDown;
        private bool lastMouseIsDownWasOnElement;


        /// <inheritdoc/>
        protected BaseTransformableParent(TEntity element, ISelectionManager<TEntity> selection) : base(element, selection)
        {

        }

        /// <inheritdoc/>
        protected BaseTransformableParent(TEntity element) : base(element)
        {

        }

        /// <summary>
        /// Called for every update of a transformation.
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <returns>A boolean value to indicate wether the update should require a rerender of the element returned by <see cref="OnTransformInvalidate"/> method.</returns>
        public abstract bool Transform(double deltaX, double deltaY);

        /// <summary>
        /// A method called during transformation to check which item to rerender.
        /// By default returns a reference to the <see cref="BaseVisualParent{TEntity}.AssociatedElement"/>
        /// </summary>
        /// <returns></returns>
        public virtual TEntity OnTransformInvalidate()
        {
            return AssociatedElement;
        }


        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonDown(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            leftMouseIsDown = true;
            lastMouseIsDownWasOnElement = CaptureMouse(LastMousePosition);

            return base.HandleLeftMouseButtonDown(invalidationRequests);
        }

        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonUp(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            leftMouseIsDown = false;
            return base.HandleLeftMouseButtonUp(invalidationRequests);
        }

        /// <inheritdoc/>
        public override bool HandleSetMousePosition(XY position, Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            var hasTransformed = false;
            if (IsSelected && leftMouseIsDown && lastMouseIsDownWasOnElement && position.DistanceTo(LastMousePosition) > DragDelta)
            {
                var deltaX = position.X - LastMousePosition.X;
                var deltaY = position.Y - LastMousePosition.Y;
                hasTransformed = Transform(deltaX, deltaY);
            }

            if (hasTransformed)
            {
                var elementToTransform = OnTransformInvalidate();
                invalidationRequests.Enqueue(new InvalidationRequest<TEntity>(elementToTransform));
            }

            return base.HandleSetMousePosition(position, invalidationRequests);
        }
    }
}
