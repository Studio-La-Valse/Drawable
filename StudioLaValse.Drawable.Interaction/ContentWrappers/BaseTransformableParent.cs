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
        public abstract InvalidationRequest<TEntity>? Transform(double deltaX, double deltaY);


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
            if (IsSelected && leftMouseIsDown && lastMouseIsDownWasOnElement && position.DistanceTo(LastMousePosition) > DragDelta)
            {
                var deltaX = position.X - LastMousePosition.X;
                var deltaY = position.Y - LastMousePosition.Y;
                if (Transform(deltaX, deltaY) is InvalidationRequest<TEntity> e)
                {
                    invalidationRequests.Enqueue(e);
                } 
            }

            return base.HandleSetMousePosition(position, invalidationRequests);
        }
    }
}
