namespace StudioLaValse.Drawable.Interaction.Selection
{
    /// <summary>
    /// The base interface for (un) selecting entities. Extends <see cref="ISelection{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISelectionManager<TEntity> : ISelection<TEntity>
    {
        /// <summary>
        /// Clears the selection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets the selection to a single element.
        /// </summary>
        /// <param name="element"></param>
        void Set(TEntity element);

        /// <summary>
        /// Sets the selection to the specified range.
        /// </summary>
        /// <param name="entities"></param>
        void SetRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds the specified element to the current selection.
        /// </summary>
        /// <param name="element"></param>
        void Add(TEntity element);

        /// <summary>
        /// Adds the specified elements to the current selection.
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the element from the selection.
        /// </summary>
        /// <param name="element"></param>
        void Remove(TEntity element);

        /// <summary>
        /// Removes the specified elements from the selection.
        /// </summary>
        /// <param name="element"></param>
        void RemoveRange(IEnumerable<TEntity> element);
    }
}
