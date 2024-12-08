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
        /// <returns>True if the selection has been changed in any way.</returns>
        /// <exception cref="InvalidSelectionProviderException"></exception>
        public virtual bool Deselect()
        {
            if (selection is null)
            {
                throw new InvalidSelectionProviderException();
            }

            return selection.Remove(AssociatedElement);
        }

        /// <summary>
        /// Select the element.
        /// </summary>
        /// <returns>True if the selection has been changed in any way.</returns>
        /// <exception cref="InvalidSelectionProviderException"></exception>
        public virtual bool Select()
        {
            if (selection is null)
            {
                throw new InvalidSelectionProviderException();
            }

            return selection.Add(AssociatedElement);
        }

        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonDown()
        {
            LastPositionOnMouseLeftDown = LastMousePosition;
            return true;
        }

        /// <inheritdoc/>
        public override bool HandleLeftMouseButtonUp()
        {
            var wasClick = LastMousePosition.DistanceTo(LastPositionOnMouseLeftDown) <= DragDelta;
            if (wasClick)
            {
                if (IsMouseOver)
                {
                    if (Select())
                    {
                        return false;
                    }
                }
                else
                {
                    if (Deselect())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
