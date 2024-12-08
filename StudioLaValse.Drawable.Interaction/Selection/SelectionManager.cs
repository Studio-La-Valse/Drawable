namespace StudioLaValse.Drawable.Interaction.Selection
{
    /// <summary>
    /// The default implementation for the <see cref="ISelectionManager{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SelectionManager<TEntity> : ISelectionManager<TEntity> where TEntity : class
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
        public static ISelectionManager<TEntity> CreateDefault<TKey>(GetKey<TEntity, TKey> getKey) where TKey : IEquatable<TKey>
        {
            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(getKey);
            var hashset = new HashSet<TEntity>(equalityComparer);
            var selection = new Selection<TEntity>(hashset);
            return new SelectionManager<TEntity>(selection, hashset);
        }

        /// <inheritdoc/>
        public bool Add(TEntity element)
        {
            return hashSet.Add(element);
        }
        /// <inheritdoc/>
        public bool Set(TEntity element)
        {
            hashSet.Clear();
            return hashSet.Add(element);
        }
        /// <inheritdoc/>
        public bool Remove(TEntity element)
        {
            return hashSet.Remove(element);
        }
        /// <inheritdoc/>
        public bool RemoveRange(IEnumerable<TEntity> elements)
        {
            var changed = false;
            foreach (var element in elements)
            {
                changed = hashSet.Remove(element);
            }
            return changed;
        }
        /// <inheritdoc/>
        public bool Clear()
        {
            if(hashSet.Count == 0)
            {
                return false;
            }

            hashSet.Clear();
            return true;
        }
        /// <inheritdoc/>
        public bool SetRange(IEnumerable<TEntity> entities)
        {
            if(entities.All(e => hashSet.Contains(e)))
            {
                return false;
            }

            hashSet.Clear();
            foreach (var element in entities)
            {
                hashSet.Add(element);
            }
            return true;
        }
        /// <inheritdoc/>
        public bool AddRange(IEnumerable<TEntity> entities)
        {
            var changed = false;
            foreach (var element in entities)
            {
                changed = hashSet.Add(element);
            }

            return changed;
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetSelection() => selection.GetSelection();
        /// <inheritdoc/>
        public bool IsSelected(TEntity element) => selection.IsSelected(element);
    }
}