namespace StudioLaValse.Drawable.Interaction.Selection
{
    /// <summary>
    /// The base interface for getting information about selected elements.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISelection<TEntity>
    {
        /// <summary>
        /// Get the selection
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetSelection();
        /// <summary>
        /// Check wheter the specified element is selected.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsSelected(TEntity element);
    }
}
