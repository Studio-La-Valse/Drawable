using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    internal class VisualTreeCache<TEntity, TKey> where TKey : IEquatable<TKey> 
                                                  where TEntity : class
    {

        private readonly Dictionary<TEntity, VisualTree<TEntity>> dict;
        private readonly GetKey<TEntity, TKey> keyExtractor;

        public VisualTreeCache(GetKey<TEntity, TKey> keyExtractor)
        {
            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(keyExtractor);
            dict = new Dictionary<TEntity, VisualTree<TEntity>>(equalityComparer);
            this.keyExtractor = keyExtractor;
        }

        public void Rebuild(VisualTree<TEntity> visualTree)
        {
            dict.Clear();
            foreach (var branch in visualTree.SelectBreadth(e => e.ChildBranches))
            {
                Add(branch.Element, branch);
            }
        }

        public void Add(TEntity entity, VisualTree<TEntity> visualTree)
        {
            if (dict.ContainsKey(entity))
            {
                throw new Exception($"Entity ({entity} : {keyExtractor(entity)}) has already been added to the visual tree.");
            }

            dict.Add(entity, visualTree);
        }

        public bool Find(TEntity entity, [NotNullWhen(true)] out VisualTree<TEntity>? value)
        {
            return dict.TryGetValue(entity, out value);
        }
    }

    internal class VisualTreeCache2<TEntity, TKey> where TKey : IEquatable<TKey>
                                                  where TEntity : class
    {

        private readonly Dictionary<VisualTree<TEntity>, InvalidationRequest<TEntity>> dict;
        private readonly GetKey<TEntity, TKey> keyExtractor;

        public VisualTreeCache2(GetKey<TEntity, TKey> keyExtractor)
        {
            var equalityComparer = new TreeBranchKeyEqualityComparer<TEntity, TKey>(keyExtractor);
            dict = new Dictionary<VisualTree<TEntity>, InvalidationRequest<TEntity>>(equalityComparer);
            this.keyExtractor = keyExtractor;
        }

        public void Rebuild(VisualTree<TEntity> visualTree, Dictionary<TEntity, InvalidationRequest<TEntity>> invalidationRequests, out IEnumerable<InvalidationRequest<TEntity>> notFound)
        {
            dict.Clear();

            void traverse(VisualTree<TEntity> _visualTree, bool parentFound)
            {
                var element = _visualTree.Element;

                var contains = invalidationRequests.TryGetValue(element, out var invalidationRequest);
                if (contains)
                {
                    if (!parentFound)
                    {
                        Add(_visualTree, invalidationRequest!);
                    }
                    invalidationRequests.Remove(element);
                }

                if (invalidationRequests.Count == 0)
                {
                    return;
                }

                foreach (var branch in _visualTree.ChildBranches)
                {
                    traverse(branch, contains);
                }
            }

            traverse(visualTree, false);

            notFound = invalidationRequests.Select(e => e.Value).ToList();
        }

        public void Add(VisualTree<TEntity> entity, InvalidationRequest<TEntity> visualTree)
        {
            if (dict.ContainsKey(entity))
            {
                throw new Exception($"Entity ({entity} : {keyExtractor(entity.Element)}) has already been added to the visual tree.");
            }

            dict.Add(entity, visualTree);
        }

        public IEnumerable<KeyValuePair<VisualTree<TEntity>, InvalidationRequest<TEntity>>> Requests() => dict;
    }
}
