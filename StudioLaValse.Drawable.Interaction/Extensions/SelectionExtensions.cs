using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;

namespace StudioLaValse.Drawable.Interaction.Extensions
{
    public static class SelectionExtensions
    {
        /// <summary>
        /// Extends the specified <see cref="ISelectionManager{TEntity}"/> to notify when it's selection has changed. The entities that are (un) selected are emitted by the specified <see cref="INotifyEntityChanged{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="selection"></param>
        /// <param name="notifyEntityChanged"></param>
        /// <returns></returns>
        public static ISelectionManager<TEntity> OnChangedNotify<TEntity>(this ISelectionManager<TEntity> selection, INotifyEntityChanged<TEntity> notifyEntityChanged) where TEntity : class, IEquatable<TEntity>
        {
            Action<IEnumerable<TEntity>, IEnumerable<TEntity>> action = (left, right) =>
            {
                notifyEntityChanged.Invalidate(left);
                notifyEntityChanged.Invalidate(right);
                notifyEntityChanged.RenderChanges();
            };
            return selection.AddChangedHandler(action);
        }

        /// <summary>
        /// Extends the specified <see cref="ISelectionManager{TEntity}"/> with a generic action when it's selection has changed. The first argument of the action are unselected elements, the second argument of the action are the selected elements.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="selection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ISelectionManager<TEntity> AddChangedHandler<TEntity>(this ISelectionManager<TEntity> selection, Action<IEnumerable<TEntity>, IEnumerable<TEntity>> action) where TEntity : class, IEquatable<TEntity>
        {
            var selectionWithHandler = new SelectionWithChangedHandler<TEntity>(selection, action);
            return selectionWithHandler;
        }
    }
}
