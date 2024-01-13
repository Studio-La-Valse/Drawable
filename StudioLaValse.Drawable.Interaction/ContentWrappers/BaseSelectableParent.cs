using StudioLaValse.Drawable.Interaction.Selection;

namespace StudioLaValse.Drawable.Interaction.ContentWrappers
{
    /// <summary>
    /// An abstract class for elements that can be selected.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseSelectableParent<TEntity> : BaseInteractiveParent<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        private readonly ISelection<TEntity>? selection;

        /// <summary>
        /// A boolean value indicating wether the associated element is currently selected.
        /// </summary>
        /// <exception cref="Exception">Thrown when no selection object is provided through the constructor, or the method is not overriden.</exception>
        public virtual bool IsSelected => selection?.IsSelected(AssociatedElement) ??
            throw new Exception("Please either provide a selection through the constructor, or override with your own logic.");


        protected BaseSelectableParent(TEntity element, ISelection<TEntity> selection) : base(element)
        {
            this.selection = selection;
        }

        public BaseSelectableParent(TEntity element) : base(element)
        {

        }
    }
}
