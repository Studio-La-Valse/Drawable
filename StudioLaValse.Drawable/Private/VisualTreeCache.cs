namespace StudioLaValse.Drawable.Private
{
    internal class VisualTreeCache<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        private readonly Dictionary<TEntity, VisualTree<TEntity>> dict = new Dictionary<TEntity, VisualTree<TEntity>>(new EntityEqualityComparer<TEntity>());

        public IEnumerable<(TEntity, VisualTree<TEntity>)> Entries => dict.Select(e => (e.Key, e.Value));

        public VisualTreeCache()
        {

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
