namespace StudioLaValse.Drawable.Interaction.Selection
{
    /// <summary>
    /// The base interface for getting information about selected elements.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISelection<TEntity>
    {
        IEnumerable<TEntity> GetSelection();
        bool IsSelected(TEntity element);
    }
}
