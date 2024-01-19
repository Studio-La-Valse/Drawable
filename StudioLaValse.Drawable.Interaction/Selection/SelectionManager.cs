namespace StudioLaValse.Drawable.Interaction.Selection
{
    /// <summary>
    /// The default implementation for the <see cref="ISelectionManager{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SelectionManager<TEntity> : ISelectionManager<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        private readonly ISelection<TEntity> selection;
        private readonly HashSet<TEntity> hashSet;

        internal SelectionManager(ISelection<TEntity> selection, HashSet<TEntity> hashSet)
        {
            this.selection = selection;
            this.hashSet = hashSet;
        }

        /// <summary>
        /// Creates the default implementation.
        /// </summary>
        /// <returns></returns>
        public static ISelectionManager<TEntity> CreateDefault()
        {
            var hashset = new HashSet<TEntity>();
            var selection = new Selection<TEntity>(hashset);
            return new SelectionManager<TEntity>(selection, hashset);
        }

        /// <inheritdoc/>
        public void Add(TEntity element)
        {
            hashSet.Add(element);
        }
        /// <inheritdoc/>
        public void Set(TEntity element)
        {
            hashSet.Clear();
            hashSet.Add(element);
        }
        /// <inheritdoc/>
        public void Remove(TEntity element)
        {
            hashSet.Remove(element);
        }
        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> elements)
        {
            foreach (var element in elements)
            {
                hashSet.Remove(element);
            }
        }
        /// <inheritdoc/>
        public void Clear()
        {
            hashSet.Clear();
        }
        /// <inheritdoc/>
        public void SetRange(IEnumerable<TEntity> entities)
        {
            hashSet.Clear();
            foreach (var element in entities)
            {
                hashSet.Add(element);
            }
        }
        /// <inheritdoc/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var element in entities)
            {
                hashSet.Add(element);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetSelection() => selection.GetSelection();
        /// <inheritdoc/>
        public bool IsSelected(TEntity element) => selection.IsSelected(element);
    }
}