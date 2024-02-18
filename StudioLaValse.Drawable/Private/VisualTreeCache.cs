namespace StudioLaValse.Drawable.Private
{
    internal class VisualTreeCache<TEntity, TKey> where TKey : IEquatable<TKey> 
                                                  where TEntity : class
    {

        private readonly Dictionary<TEntity, VisualTree<TEntity>> dict;

        public IEnumerable<(TEntity, VisualTree<TEntity>)> Entries => dict.Select(e => (e.Key, e.Value));

        public VisualTreeCache(GetKey<TEntity, TKey> keyExtractor)
        {
            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(keyExtractor);
            dict = new Dictionary<TEntity, VisualTree<TEntity>>(equalityComparer);
        }

        public void Rebuild(VisualTree<TEntity> visualTree)
        {
            dict.Clear();
            foreach (var branch in visualTree.SelectRecursive(e => e.ChildBranches))
            {
                Add(branch.Element, branch);
            }
        }

        public void Add(TEntity entity, VisualTree<TEntity> visualTree)
        {
            if (dict.ContainsKey(entity))
            {
                throw new Exception("Entity has already been added to the visual tree.");
            }

            dict.Add(entity, visualTree);
        }

        public VisualTree<TEntity> FindOrThrow(TEntity entity)
        {
            if (dict.TryGetValue(entity, out var value))
            {
                return value;
            }

            throw new Exception("Specified entity was not found in the visual tree.");
        }
    }
}
