namespace StudioLaValse.Drawable.Interaction.Selection
{
    internal class Selection<TEntity> : ISelection<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        private readonly HashSet<TEntity> selection;

        internal Selection(HashSet<TEntity> selection)
        {
            this.selection = selection;
        }

        public bool IsSelected(TEntity element)
        {
            return GetSelection().Any(e => e.Equals(element));
        }
        public IEnumerable<TEntity> GetSelection()
        {
            return selection;
        }
    }
}