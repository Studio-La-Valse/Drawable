using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Private;

namespace StudioLaValse.Drawable.Interaction.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="ISelectionManager{TEntity}"/> interface.
    /// </summary>
    public static class SelectionExtensions
    {
        /// <summary>
        /// Extends the specified <see cref="ISelectionManager{TEntity}"/> to notify when it's selection has changed. The entities that are (un) selected are emitted by the specified <see cref="INotifyEntityChanged{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selection"></param>
        /// <param name="notifyEntityChanged"></param>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public static ISelectionManager<TEntity> OnChangedNotify<TEntity, TKey>(this ISelectionManager<TEntity> selection, INotifyEntityChanged<TEntity> notifyEntityChanged, GetKey<TEntity, TKey> getKey) where TEntity : class where TKey : IEquatable<TKey>
        {
            void action(IEnumerable<TEntity> left, IEnumerable<TEntity> right)
            {
                notifyEntityChanged.Invalidate(left, NotFoundHandler.Skip, Method.Recursive);
                notifyEntityChanged.Invalidate(right, NotFoundHandler.Skip, Method.Recursive);
                // notifyEntityChanged.RenderChanges();
            }
            return selection.AddChangedHandler(action, getKey);
        }

        /// <summary>
        /// Extends the specified <see cref="ISelectionManager{TEntity}"/> with a generic action when it's selection has changed. The first argument of the action are unselected elements, the second argument of the action are the selected elements.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selection"></param>
        /// <param name="action"></param>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public static ISelectionManager<TEntity> AddChangedHandler<TEntity, TKey>(this ISelectionManager<TEntity> selection, Action<IEnumerable<TEntity>, IEnumerable<TEntity>> action, GetKey<TEntity, TKey> getKey) where TEntity : class where TKey : IEquatable<TKey>
        {
            var selectionWithHandler = new SelectionWithChangedHandler<TEntity, TKey>(selection, action, getKey);
            return selectionWithHandler;
        }

        /// <summary>
        /// Extends the specified <see cref="ISelectionManager{TEntity}"/> to intercept keyboard keys.
        /// When setting the selection:
        ///     if shift is pressed, the selection is appended,
        ///     if control is pressed, the selection is removed.
        /// Pressing (releasing) escape clears the selection.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="selectionManager"></param>
        /// <returns></returns>
        public static SelectionWithKeyResponse<TEntity> InterceptKeys<TEntity>(this ISelectionManager<TEntity> selectionManager) where TEntity : class
        {
            var selectionWithKeys = new SelectionWithKeyResponse<TEntity>(selectionManager);
            return selectionWithKeys;
        }
    }
}
