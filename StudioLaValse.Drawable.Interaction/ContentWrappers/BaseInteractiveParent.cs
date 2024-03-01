using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{

    /// <summary>
    /// An abstract class meant to be used for a visual parent that needs any basic mouse interaction. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseInteractiveParent<TEntity> : BaseVisualParent<TEntity> where TEntity : class
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
        public bool IsMouseOver { get; set; }


        /// <inheritdoc/>
        protected BaseInteractiveParent(TEntity element) : base(element)
        {

        }

        /// <summary>
        /// Called when mouse cursor moves in an area where <see cref="Respond(XY)"/> returns true for the same position.
        /// Use the implementation to do whatever you want with the attached entity.
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns>A boolean when if true, triggers a render invalidation for the specified <see cref="Ghost"/>.</returns>
        public virtual bool OnMouseMove(XY mousePosition)
        {
            var mouseCurrentlyOver = IsMouseOver;
            IsMouseOver = Respond(mousePosition);
            return mouseCurrentlyOver != IsMouseOver;
        }

        /// <summary>
        /// An abstract method called to check wether the current mouse position should trigger any kind of response.
        /// By default, this method returns true if the position is contained in the default bounding box of the content wrapper.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Respond(XY point)
        {
            return BoundingBox().Contains(point);
        }
    }
}
