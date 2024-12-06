using StudioLaValse.Drawable.Interaction.Exceptions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// An abstract class for elements that can be selected.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseSelectableParent<TEntity> : BaseInteractiveParent<TEntity> where TEntity : class
    {
        private readonly ISelectionManager<TEntity>? selection;

        /// <summary>
        /// A boolean value indicating wether the associated element is currently selected.
        /// </summary>
        /// <exception cref="Exception">Thrown when no selection object is provided through the constructor, or the method is not overriden.</exception>
        public virtual bool IsSelected => selection?.IsSelected(AssociatedElement) ??
            throw new InvalidSelectionProviderException();

        /// <summary>
        /// The last position when the left mouse button was pressed.
        /// </summary>
        protected XY LastPositionOnMouseLeftDown { get; set; }

        /// <summary>
        /// The minimum drag before a drag transformation is performed.
        /// </summary>
        protected virtual double DragDelta { get; set; } = 2;

        /// <inheritdoc/>
        protected BaseSelectableParent(TEntity element, ISelectionManager<TEntity> selection) : base(element)
        {
            this.selection = selection;
        }

        /// <inheritdoc/>
        public BaseSelectableParent(TEntity element) : base(element)
        {

        }


        /// <summary>
        /// Deselect the element.
        /// Returns true if the selection has been succesfully changed.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidSelectionProviderException"></exception>
        public virtual InvalidationRequest<TEntity>? Deselect()
        {
            if (selection is null)
            {
                throw new InvalidSelectionProviderException();
            }

            if (!selection.IsSelected(AssociatedElement))
            {
                return null;
            }

            selection.Remove(AssociatedElement);
            return new InvalidationRequest<TEntity>(AssociatedElement);
        }

        /// <summary>
        /// Select the element.
        /// Returns true if the selection has been succesfully changed.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidSelectionProviderException"></exception>
        public virtual InvalidationRequest<TEntity>? Select()
        {
            if (selection is null)
            {
                throw new InvalidSelectionProviderException();
            }

            if (selection.IsSelected(AssociatedElement))
            {
                return null;
            }

            selection.Set(AssociatedElement);
            return new InvalidationRequest<TEntity>(AssociatedElement);
        }

        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonDown(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            LastPositionOnMouseLeftDown = LastMousePosition;
            return true;
        }

        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonUp(Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            var wasClick = LastMousePosition.DistanceTo(LastPositionOnMouseLeftDown) <= DragDelta;
            if (wasClick)
            {
                if (IsMouseOver)
                {
                    if (Select() is InvalidationRequest<TEntity> e)
                    {
                        invalidationRequests.Enqueue(e);
                        return false;
                    }
                }
                else
                {
                    if (Deselect() is InvalidationRequest<TEntity> e)
                    {
                        invalidationRequests.Enqueue(e);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool HandleSetMousePosition(XY position, Queue<InvalidationRequest<TEntity>> invalidationRequests)
        {
            LastMousePosition = position;

            return base.HandleSetMousePosition(position, invalidationRequests);
        }
    }
}
