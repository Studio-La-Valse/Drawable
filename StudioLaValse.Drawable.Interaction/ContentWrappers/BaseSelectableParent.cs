using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// An exception that is thrown when an invalid selection logic has been provided.
    /// </summary>
    public class InvalidSelectionProviderException : Exception
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidSelectionProviderException() : base("Please either provide a selection through the constructor, or override with your own logic.")
        {
            
        }
    }

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

        /// <summary>
        /// Deselect the element.
        /// Returns true if the selection has been succesfully changed.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidSelectionProviderException"></exception>
        public virtual bool Deselect()
        {
            if(selection is null)
            {
                throw new InvalidSelectionProviderException();
            }

            if (!selection.IsSelected(AssociatedElement))
            {
                return false;
            }

            selection.Remove(AssociatedElement);
            return true;
        }

        /// <summary>
        /// Select the element.
        /// Returns true if the selection has been succesfully changed.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidSelectionProviderException"></exception>
        public virtual bool Select()
        {
            if (selection is null)
            {
                throw new InvalidSelectionProviderException();
            }

            if (selection.IsSelected(AssociatedElement))
            {
                return false;
            }

            selection.Set(AssociatedElement);
            return true;
        }

        /// <inheritdoc/>
        protected BaseSelectableParent(TEntity element, ISelectionManager<TEntity> selection) : base(element)
        {
            this.selection = selection;
        }

        /// <inheritdoc/>
        public BaseSelectableParent(TEntity element) : base(element)
        {

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
                    if (Select())
                    {
                        invalidationRequests.Enqueue(new InvalidationRequest<TEntity>(AssociatedElement));
                    }
                }
                else
                {
                    if (Deselect())
                    {
                        invalidationRequests.Enqueue(new InvalidationRequest<TEntity>(AssociatedElement));
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
