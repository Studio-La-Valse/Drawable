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

        /// <summary>
        /// A flag that prevents element transformation on mouse drag.
        /// </summary>
        public bool LockTransform { get; set; }

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
        public abstract void Transform(double deltaX, double deltaY);


        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonDown()
        {
            leftMouseIsDown = true;
            lastMouseIsDownWasOnElement = CaptureMouse(LastMousePosition);

            return base.HandleLeftMouseButtonDown();
        }

        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonUp()
        {
            leftMouseIsDown = false;
            return base.HandleLeftMouseButtonUp();
        }

        /// <inheritdoc/>
        public override bool HandleSetMousePosition(XY position)
        {
            if (!LockTransform && leftMouseIsDown && lastMouseIsDownWasOnElement && IsSelected && position.DistanceTo(LastMousePosition) > DragDelta)
            {
                var deltaX = position.X - LastMousePosition.X;
                var deltaY = position.Y - LastMousePosition.Y;
                Transform(deltaX, deltaY);
            }

            return base.HandleSetMousePosition(position);
        }
    }
}
