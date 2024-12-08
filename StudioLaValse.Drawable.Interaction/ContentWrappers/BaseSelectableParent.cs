using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// An abstract class for elements that can be selected.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseSelectableParent<TKey> : BaseInteractiveParent<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// A boolean value indicating wether the associated element is currently selected.
        /// </summary>
        /// <exception cref="Exception">Thrown when no selection object is provided through the constructor, or the method is not overriden.</exception>
        public abstract bool IsSelected { get; }

        /// <summary>
        /// The last position when the left mouse button was pressed.
        /// </summary>
        protected XY LastPositionOnMouseLeftDown { get; set; }

        /// <summary>
        /// The minimum drag before a drag transformation is performed.
        /// </summary>
        protected virtual double DragDelta { get; set; } = 2;

        /// <inheritdoc/>
        public BaseSelectableParent(TKey element) : base(element)
        {

        }


        /// <summary>
        /// Deselect the element.
        /// Returns true if the selection has been succesfully changed.
        /// </summary>
        /// <returns>True if the selection has been changed in any way.</returns>
        public abstract bool Deselect();

        /// <summary>
        /// Select the element.
        /// </summary>
        /// <returns>True if the selection has been changed in any way.</returns>
        public abstract bool Select();

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
