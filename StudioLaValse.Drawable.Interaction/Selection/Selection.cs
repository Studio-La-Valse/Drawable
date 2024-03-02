namespace StudioLaValse.Drawable.Interaction.Selection
{
    internal class Selection<TEntity> : ISelection<TEntity> where TEntity : class
    {
        private readonly HashSet<TEntity> selection;

        internal Selection(HashSet<TEntity> selection)
        {
            this.selection = selection;
        }

        public bool IsSelected(TEntity element)
        {
            return selection.Contains(element);
        }
        public IEnumerable<TEntity> GetSelection()
        {
            return selection;
        }
    }
}