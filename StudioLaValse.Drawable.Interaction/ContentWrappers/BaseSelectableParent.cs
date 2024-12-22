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
        /// The minimum drag before a drag transformation is performed.
        /// </summary>
        protected virtual double DragDelta { get; set; } = 2;

        /// <summary>
        /// A boolean flag that inidicates if the mouse is now over the element.
        /// </summary>
        protected virtual bool IsMouseOver { get; set; }

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

        /// <summary>
        /// Custom logic on mouse enter.
        /// </summary>
        /// <returns></returns>
        public virtual void OnMouseEnter()
        {
            IsMouseOver = true;
        }

        /// <summary>
        /// Custom logic on mouse enter.
        /// </summary>
        /// <returns></returns>
        public virtual void OnMouseLeave()
        {
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
        public override bool HandleMouseMove(XY position)
        {
            LastMousePosition = position;

            var wasOverBefore = IsMouseOver;
            var isNowOver = CaptureMouse(position);

            if (wasOverBefore != isNowOver)
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
